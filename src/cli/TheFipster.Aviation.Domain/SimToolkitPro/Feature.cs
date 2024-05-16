using System.Text.Json.Serialization;

namespace TheFipster.Aviation.Domain.SimToolkitPro
{
    public class Feature
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("geometry")]
        public Geometry Geometry { get; set; }
    }
}
