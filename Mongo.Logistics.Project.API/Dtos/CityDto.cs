using Newtonsoft.Json;

namespace Mongo.Logistics.Project.API.Dtos
{
    public class CityDto
    {
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("location")]
        public double[] Location { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
