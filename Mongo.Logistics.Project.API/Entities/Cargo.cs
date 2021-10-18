using Mongo.Logistics.Project.API.DAL;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Mongo.Logistics.Project.API.Entities
{
    [BsonCollection("cargo")]
    public class Cargo
    {
        [BsonElement("_id")]
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("location")]
        public string Location { get; set; }

        [BsonElement("locationType")]
        public string LocationType { get; set; }

        [BsonElement("destination")]
        public string Destination { get; set; }

        [BsonElement("courier")]
        public string Courier { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("received")]
        public DateTime? Received { get; set; }
    }
}
