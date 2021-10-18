using System;
using System.Linq;

namespace Mongo.Logistics.Project.API.DAL.Configuration
{
    public static class MongoHelper
    {
        public static string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }
    }
}
