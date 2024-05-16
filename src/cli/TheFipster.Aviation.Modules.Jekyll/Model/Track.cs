using System.Text.Json.Serialization;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    internal class Track
    {
        public Track(string flightName, string uri, List<List<decimal>> latLonCoordinates)
        {
            Flight = flightName;
            FlightUri = uri;
            Coordinates = latLonCoordinates;
        }

        [JsonPropertyName("flt")]
        public string Flight { get; set; }

        [JsonPropertyName("uri")]
        public string FlightUri { get; set; }

        [JsonPropertyName("gps")]
        public List<List<decimal>> Coordinates { get; set; }
    }
}