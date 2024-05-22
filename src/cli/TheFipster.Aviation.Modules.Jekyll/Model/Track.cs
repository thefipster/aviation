using System.Text.Json.Serialization;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    internal class Track
    {
        public Track(int flightNo, string flightName, string uri, List<List<decimal>> latLonCoordinates)
        {
            FlightNo = flightNo;
            Flight = flightName;
            FlightUri = uri;
            Coordinates = latLonCoordinates;
        }

        [JsonPropertyName("no")]
        public int FlightNo { get; set; }

        [JsonPropertyName("flt")]
        public string Flight { get; set; }

        [JsonPropertyName("uri")]
        public string FlightUri { get; set; }

        [JsonPropertyName("gps")]
        public List<List<decimal>> Coordinates { get; set; }
    }
}