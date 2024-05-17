using System.Text.Json.Serialization;
using TheFipster.Aviation.Domain.OurAirports;

namespace TheFipster.Aviation.Domain
{
    public class AirportMeta
    {
        public AirportMeta(OurAirport airport)
        {
            Icao = airport.Ident;
            Coordinates = [Math.Round(Convert.ToDecimal(airport.Latitude), 4), Math.Round(Convert.ToDecimal(airport.Longitude), 4)];

            Type = airport.Type switch
            {
                "small_airport" => 0,
                "medium_airport" => 1,
                "large_airport" => 2,
                "seaplane_base" => 10,
                "heliport" => 20,
                _ => throw new Exception("In your face motherfucker")
            };
        }

        public AirportMeta(string icao, string type, double latitude, double longitude)
        {
            Icao = icao;
            Coordinates = [Math.Round(Convert.ToDecimal(latitude), 4), Math.Round(Convert.ToDecimal(longitude), 4)];
            Type = type switch
            {
                "small_airport" => 0,
                "medium_airport" => 1,
                "large_airport" => 2,
                "seaplane_base" => 10,
                "heliport" => 20,
                _ => throw new Exception("In your face motherfucker")
            };
        }

        [JsonPropertyName("t")]
        public int Type { get; set; }

        [JsonPropertyName("i")]
        public string Icao { get; set; }

        [JsonPropertyName("p")]
        public decimal[] Coordinates { get; set; }
    }
}
