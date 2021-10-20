using Mongo.Logistics.Project.API.DAL;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Mongo.Logistics.Project.API.Entities
{
    [BsonCollection("planesLandedPlaces")]
    public class PlaneLandedPlaces
    {
        [BsonElement("_id")]
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("plane")]
        public string Plane { get; set; }

        [BsonElement("landedCity")]
        public string LandedCity { get; set; }

        [BsonElement("landedLocation")]
        public double[] LandedLocation { get; set; }

        [BsonElement("previousLandedCity")]
        public string PreviousLandedCity { get; set; }

        [BsonElement("previousLandedLocation")]
        public double[] PreviousLandedLocation { get; set; }

        [BsonElement("landedDate")]
        public DateTime LandedDate { get; set; }

        [BsonElement("secondsFromLastLanded")]
        [BsonRepresentation(BsonType.Int32, AllowTruncation = true)]
        public int SecondsFromLastLanded { get; set; }

        [BsonElement("milesDistanceFromLastLanded")]
        [BsonRepresentation(BsonType.Double, AllowTruncation = true)]
        public double MilesDistanceFromLastLanded { get; set; }

    }
}
