using System.Globalization;
using TheFipster.Aviation.Domain.Datahub;

namespace TheFipster.Aviation.FlightApi.Models
{
    public class Point
    {
        public Point(Airport departure)
        {
            Icao = departure.Ident;
            Name = departure.Name;

            // coordinate is in format lon, lat
            var split = departure.Coordinates.Split(',');
            Longitude = double.Parse(split[0].Trim(), CultureInfo.InvariantCulture);
            Latitude = double.Parse(split[1].Trim(), CultureInfo.InvariantCulture);
        }

        public Point(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        public Point(string icao, string name, double latitude, double longitude)
            : this(name, latitude, longitude)
        {
            Icao = icao;
        }

        public string Icao { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
