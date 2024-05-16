using System.Text.Json.Serialization;

namespace TheFipster.Aviation.Domain.SimToolkitPro
{
    public class Geometry
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("coordinates")]
        public List<List<double>> Coordinates { get; set; }
    }
}
