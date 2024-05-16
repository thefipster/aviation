using System;
using System.Text.Json.Serialization;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    internal class FlightGeo
    {
        [JsonPropertyName("e")]
        public IEnumerable<Location> Events { get; set; }

        [JsonPropertyName("wp")]
        public IEnumerable<Location> Waypoints { get; set; }

        [JsonPropertyName("trk")]
        public IEnumerable<IEnumerable<decimal>> Track { get; set; }

        [JsonPropertyName("img")]
        public IEnumerable<UriLocation> Images { get; set; }
    }
}
