using System.Text.Json.Serialization;

namespace TheFipster.Aviation.Domain.SimToolkitPro
{
    public class Track : JsonBase
    {
        public Track()
        {
            Features = new List<Feature>();
        }

        [JsonPropertyName("departure")]
        public string? Departure { get; set; }

        [JsonPropertyName("arrival")]
        public string? Arrival { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("features")]
        public List<Feature> Features { get; set; }
    }
}
