using Mongo.Logistics.Project.API.DAL.Configuration;
using Mongo.Logistics.Project.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mongo.Logistics.Project.API.DAL
{
    public class PlaneRepository
    {
        private readonly IMongoCollection<Plane> _collection;
        private readonly FindOneAndUpdateOptions<Plane> defaultOptions;
        public PlaneRepository(MongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Plane>(MongoHelper.GetCollectionName(typeof(Plane)));
            defaultOptions = new FindOneAndUpdateOptions<Plane> { ReturnDocument = ReturnDocument.After };
        }



        public async Task<Plane> GetByIdAsync(string id)
        {
            return await _collection.Find(Builders<Plane>.Filter.Eq(doc => doc.Id, id)).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Plane>> GetAllAsync()
        {
            return await _collection.Find(Builders<Plane>.Filter.Empty).ToListAsync();
        }

        public async Task<Plane> ReplaceOneAsync(Plane document)
        {
            var filter = Builders<Plane>.Filter.Eq(doc => doc.Id, document.Id);
            return await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public async Task<Plane> UpdadeOneAsync(string id, UpdateDefinition<Plane> update)
        {
            var filter = Builders<Plane>.Filter.Eq(doc => doc.Id, id);
            return await _collection.FindOneAndUpdateAsync(filter, update, defaultOptions);
        }

    }
}
