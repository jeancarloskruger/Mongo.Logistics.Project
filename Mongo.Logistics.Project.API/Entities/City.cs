using Mongo.Logistics.Project.API.DAL;
using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Logistics.Project.API.Entities
{
    [BsonCollection("cities")]
    public class City
    {
        [BsonElement("_id")]
        [BsonId]
        public string Id { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("position")]
        public double[] Position { get; set; }


    }
}
