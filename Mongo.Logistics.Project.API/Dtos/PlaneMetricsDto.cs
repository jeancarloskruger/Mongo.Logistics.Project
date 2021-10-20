using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Mongo.Logistics.Project.API.Dtos
{
    public class PlaneMetricsDto
    {
        [JsonProperty("_id")]
        [BsonElement("_id")]
        public string Id { get; set; }

        [JsonProperty("totalMiles")]
        [BsonElement("totalMiles")]
        public double? TotalMiles { get; set; }

        [JsonProperty("timeFlyingSeconds")]
        [BsonElement("timeFlyingSeconds")]
        public int? TimeFlyingSeconds { get; set; }

        [JsonProperty("lastPlaces")]
        [BsonElement("lastPlaces")]
        public string[] LastPlaces { get; set; }
    }
}
