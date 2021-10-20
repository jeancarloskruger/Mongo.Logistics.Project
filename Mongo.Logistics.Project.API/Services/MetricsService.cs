using Mongo.Logistics.Project.API.Consts;
using Mongo.Logistics.Project.API.DAL;
using Mongo.Logistics.Project.API.DAL.Configuration;
using Mongo.Logistics.Project.API.Dtos;
using Mongo.Logistics.Project.API.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Logistics.Project.API.Services
{
    public class MetricsService
    {
        private readonly IMongoCollection<PlaneLandedPlaces> planeLandedPlacesCollection;

        public MetricsService(MongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            planeLandedPlacesCollection = database.GetCollection<PlaneLandedPlaces>(MongoHelper.GetCollectionName(typeof(PlaneLandedPlaces)));

        }

        public async Task<List<PlaneMetricsDto>> GetPlaneMetrics()
        {
            var sort = new BsonDocument("$sort", new BsonDocument("landedDate", -1));

            var group = new BsonDocument("$group",
                                    new BsonDocument
                                        {
                                            { "_id", "$plane" },
                                            { "totalMiles",
                                    new BsonDocument("$sum", "$milesDistanceFromLastLanded") },
                                            { "timeFlyingSeconds",
                                    new BsonDocument("$sum", "$secondsFromLastLanded") },
                                            { "lastPlaces",
                                    new BsonDocument("$push", "$$ROOT.landedCity") }
                                        });

            var addField = new BsonDocument("$addFields",
                                        new BsonDocument("lastPlaces",
                                        new BsonDocument("$slice",
                                        new BsonArray
                                                    {
                                                        "$lastPlaces",
                                                        MetricsConst.SizeOfListLastLocations
                                                    })));


            var pipeline = new List<BsonDocument>();
            pipeline.Add(sort);
            pipeline.Add(group);
            pipeline.Add(addField);

            return await planeLandedPlacesCollection.Aggregate<PlaneMetricsDto>(pipeline).ToListAsync();

        }
    }
}
