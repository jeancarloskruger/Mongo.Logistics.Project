using Mongo.Logistics.Project.API.DAL.Configuration;
using Mongo.Logistics.Project.API.Entities;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
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
            var filterPoint = GeoJson.Point(new GeoJson2DCoordinates(longitude, latitude));

            var filter = new FilterDefinitionBuilder<City>()
                         .NearSphere(n => n.Position, filterPoint, minDistance: 1);

            return await _collection.Find(filter).Limit(count).ToListAsync();
        }
    }
}
