using Mongo.Logistics.Project.API.Consts;
using Mongo.Logistics.Project.API.DAL;
using Mongo.Logistics.Project.API.DAL.Configuration;
using Mongo.Logistics.Project.API.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Logistics.Project.API.Services
{
    public class ChangeStreamService
    {
        private readonly IMongoCollection<Plane> planeCollection;
        private readonly IMongoCollection<PlaneLandedPlaces> planeLandedPlacesCollection;
        private readonly IMongoCollection<City> cityCollection;
        private readonly ChangeStreamOptions changeStreamOptions;
        public ChangeStreamService(MongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            planeCollection = database.GetCollection<Plane>(MongoHelper.GetCollectionName(typeof(Plane)));
            planeLandedPlacesCollection = database.GetCollection<PlaneLandedPlaces>(MongoHelper.GetCollectionName(typeof(PlaneLandedPlaces)));
            cityCollection = database.GetCollection<City>(MongoHelper.GetCollectionName(typeof(City)));
            changeStreamOptions = new ChangeStreamOptions
            {
                FullDocument = ChangeStreamFullDocumentOption.UpdateLookup,
                BatchSize = 1
            };
        }

        public async Task MonitoringPlaneLanded()
        {
            var filter = Builders<ChangeStreamDocument<Plane>>
                .Filter.Where(x =>
                    x.OperationType == ChangeStreamOperationType.Update);

            filter &= Builders<ChangeStreamDocument<Plane>>.Filter.Exists("updateDescription.updatedFields.landed");

            var pipelineChangeStrem = new IPipelineStageDefinition[]
            {
               PipelineStageDefinitionBuilder.Match(filter)
            };

            using (var cursor = await planeCollection.WatchAsync<ChangeStreamDocument<Plane>>(pipelineChangeStrem, changeStreamOptions))
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var change in cursor.Current)
                    {
                        var match = new BsonDocument("$match", new BsonDocument("plane", change.FullDocument.Id));

                        var sort = new BsonDocument("$sort", new BsonDocument("landedDate", -1));

                        var limit = new BsonDocument("$limit", 1);

                        var projection = new BsonDocument("$project",
                                        new BsonDocument
                                            {
                                            { "_id", 0 },
                                            { "plane", change.FullDocument.Id },
                                            { "landedCity",  change.FullDocument.Landed },
                                            { "landedLocation",  new BsonArray{ change.FullDocument.CurrentLocation[0], change.FullDocument.CurrentLocation[1]}},
                                            { "secondsFromLastLanded",
                                                    new BsonDocument("$divide",
                                                    new BsonArray
                                                                {
                                                                    new BsonDocument("$subtract",
                                                                    new BsonArray
                                                                        {
                                                                         change.FullDocument.LandedDate,
                                                                         "$landedDate"
                                                                        }),
                                                                    ConverterMeasuresConst.MillisecondsToSeconds
                                                                }) },
                                            { "landedDate", change.FullDocument.LandedDate },
                                            { "previousLandedCity", "$landedCity" },
                                            { "previousLandedLocation", "$landedLocation" }
                                        });

                        var pipeline = new List<BsonDocument>();
                        pipeline.Add(match);
                        pipeline.Add(sort);
                        pipeline.Add(limit);
                        pipeline.Add(projection);

                        var newPlanePlaceEntry = planeLandedPlacesCollection.Aggregate<PlaneLandedPlaces>(pipeline).FirstOrDefault();

                        if (newPlanePlaceEntry != null)
                        {
                            newPlanePlaceEntry.MilesDistanceFromLastLanded = await GetMilesDistance(newPlanePlaceEntry);
                            newPlanePlaceEntry.Id = ObjectId.GenerateNewId();
                            planeLandedPlacesCollection.InsertOne(newPlanePlaceEntry);
                        }
                        else
                        {
                            planeLandedPlacesCollection.InsertOne(new PlaneLandedPlaces
                            {
                                Plane = change.FullDocument.Id,
                                LandedCity = change.FullDocument.Landed,
                                LandedDate = change.FullDocument.LandedDate,
                                LandedLocation = change.FullDocument.CurrentLocation,
                                MilesDistanceFromLastLanded = 0,
                                SecondsFromLastLanded = 0
                            });
                        }
                    }
                }
            }
        }

        private async Task<double> GetMilesDistance(PlaneLandedPlaces newPlanePlaceEntry)
        {
            if (!string.IsNullOrWhiteSpace(newPlanePlaceEntry.PreviousLandedCity))
            {
                var geoNearOptions = new BsonDocument
                                                {
                                                    {
                                                        "near",
                                                        new BsonDocument
                                                        {
                                                            { "type", "Point" },
                                                            { "coordinates", new BsonArray{ newPlanePlaceEntry.LandedLocation[0], newPlanePlaceEntry.LandedLocation[1] } }
                                                        }
                                                    },
                                                    { "distanceField", "milesDistance" },
                                                    { "distanceMultiplier", ConverterMeasuresConst.MetersToMiles },
                                                    { "query",  new BsonDocument("_id",newPlanePlaceEntry.PreviousLandedCity) },
                                                    { "spherical", false }
                                                };

                var projectOptions = new BsonDocument
                                                {
                                                    { "_id" , 0 },
                                                    { "milesDistance", 1 }
                                                };


                var geoPipeline = new List<BsonDocument>();
                geoPipeline.Add(new BsonDocument { { "$geoNear", geoNearOptions } });
                geoPipeline.Add(new BsonDocument { { "$project", projectOptions } });

                var result = await cityCollection.Aggregate<BsonDocument>(geoPipeline).FirstOrDefaultAsync();

                if (result != null)
                {
                    return result.GetValue("milesDistance").AsDouble;
                }
            }
            return 0;
        }

        public async Task MonitoringPlaneMaintance()
        {
            var filter = Builders<ChangeStreamDocument<PlaneLandedPlaces>>
                .Filter.Where(x =>
                    x.OperationType == ChangeStreamOperationType.Insert);

            var pipelineChangeStrem = new IPipelineStageDefinition[]
           {
               PipelineStageDefinitionBuilder.Match(filter)
           };

            using (var cursor = await planeLandedPlacesCollection.WatchAsync<ChangeStreamDocument<PlaneLandedPlaces>>(pipelineChangeStrem, changeStreamOptions))
            {
                while (await cursor.MoveNextAsync())
                {
                    foreach (var change in cursor.Current)
                    {
                        var group = new BsonDocument("$group",
                                           new BsonDocument
                                               {
                                                    { "_id", "$plane" },
                                                    { "totalMiles",
                                            new BsonDocument("$sum", "$milesDistanceFromLastLanded") }
                                               });

                        var match = new BsonDocument("$match",
                                            new BsonDocument("totalMiles",
                                            new BsonDocument("$gt", MaintenancePolicyConst.MaxMilesBeforeMoveToMaintenance)));

                        var lookup = new BsonDocument("$lookup",
                                        new BsonDocument
                                            {
                                                { "from", "planes" },
                                                { "localField", "_id" },
                                                { "foreignField", "_id" },
                                                { "as", "plane" }
                                            });

                        var unwind = new BsonDocument("$unwind", new BsonDocument("path", "$plane"));

                        var matchPlaneReadyToMaintenanceRequired = new BsonDocument("$match",
                                                    new BsonDocument("$or",
                                                    new BsonArray
                                                            {
                                                                new BsonDocument("plane.maintenanceRequired",
                                                                new BsonDocument
                                                                    {
                                                                        { "$exists", true },
                                                                        { "$ne", true }
                                                                    }),
                                                                new BsonDocument("plane.maintenanceRequired",
                                                                new BsonDocument("$exists", false))
                                                            }));

                        var pipeline = new List<BsonDocument>();
                        pipeline.Add(group);
                        pipeline.Add(match);
                        pipeline.Add(lookup);
                        pipeline.Add(unwind);
                        pipeline.Add(matchPlaneReadyToMaintenanceRequired);

                        var planes = await planeLandedPlacesCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

                        if (planes != null)
                        {
                            foreach (var item in planes)
                            {
                                var filterPlane = Builders<Plane>.Filter.Eq(doc => doc.Id, item.GetValue("_id"));
                                var update = Builders<Plane>.Update
                                    .Set(s => s.MaintenanceRequired, true);
                                await planeCollection.FindOneAndUpdateAsync(filterPlane, update);
                            }
                        }
                    }
                }
            }
        }
    }
}
