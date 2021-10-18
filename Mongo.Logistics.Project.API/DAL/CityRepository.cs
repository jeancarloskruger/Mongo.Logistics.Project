using Mongo.Logistics.Project.API.DAL.Configuration;
using Mongo.Logistics.Project.API.Entities;
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
            return await _collection.Find(Builders<City>.Filter.NearSphere(doc => doc.Position, longitude, latitude)).Limit(count).ToListAsync();
        }
    }
}
