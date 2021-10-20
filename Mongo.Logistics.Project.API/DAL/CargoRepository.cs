using Mongo.Logistics.Project.API.Consts;
using Mongo.Logistics.Project.API.DAL.Configuration;
using Mongo.Logistics.Project.API.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Logistics.Project.API.DAL
{
    public class CargoRepository
    {
        private readonly IMongoCollection<Cargo> _collection;
        private readonly FindOneAndUpdateOptions<Cargo> defaultOptions;

        public CargoRepository(MongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Cargo>(MongoHelper.GetCollectionName(typeof(Cargo)));
            defaultOptions = new FindOneAndUpdateOptions<Cargo> { ReturnDocument = ReturnDocument.After };
        }

        public async Task<Cargo> GetByIdAsync(string id)
        {
            var objectId = new ObjectId(id);
            return await _collection.Find(Builders<Cargo>.Filter.Eq(doc => doc.Id, objectId)).FirstOrDefaultAsync();
        }

        public async Task InsertOneAsync(Cargo cargo)
        {
            await _collection.InsertOneAsync(cargo);
        }

        public async Task<Cargo> UpdadeOneAsync(ObjectId id, UpdateDefinition<Cargo> update)
        {
            var filter = Builders<Cargo>.Filter.Eq(doc => doc.Id, id);
            return await _collection.FindOneAndUpdateAsync(filter, update, defaultOptions);
        }

        public async Task<List<Cargo>> GetAllInProgressByLocation(string location)
        {
            var filter = Builders<Cargo>.Filter.Eq(doc => doc.Location, location) & Builders<Cargo>.Filter.Eq(doc => doc.Status, CargoStatusConst.InProgress);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
