using Mongo.Logistics.Project.API.DAL.Configuration;
using Mongo.Logistics.Project.API.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Logistics.Project.API.DAL
{
    public class CityRepository
    {
        private readonly IMongoCollection<City> _collection;

        public CityRepository(MongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<City>(MongoHelper.GetCollectionName(typeof(City)));
        }

        public async Task<City> GetByIdAsync(string id)
        {
            return await _collection.Find(Builders<City>.Filter.Eq(doc => doc.Id, id)).FirstOrDefaultAsync();
        }

        public async Task<ICollection<City>> GetAllAsync()
        {
            return await _collection.Find(Builders<City>.Filter.Empty).ToListAsync();
        }

        public async Task<ICollection<City>> GetNearbyCitiesSortedByNearestFirst(double longitude, double latitude, int count)
        {
            var geoNear = new BsonDocument("$geoNear",
                                    new BsonDocument
                                        {
                                            { "near", new BsonDocument
                                            {
                                                { "type", "Point" },
                                                { "coordinates", new BsonArray { longitude, latitude} }
                                            }
                                        },
                                            { "distanceField", "distance" },
                                            { "minDistance", 1 },
                                            { "spherical", false }
                                        });

            var limit = new BsonDocument("$limit", count);

            var projection = new BsonDocument("$project", new BsonDocument("distance", 0));

            var pipeline = new List<BsonDocument>();
            pipeline.Add(geoNear);
            pipeline.Add(limit);
            pipeline.Add(projection);

            return await _collection.Aggregate<City>(pipeline).ToListAsync();
        }
    }
}
