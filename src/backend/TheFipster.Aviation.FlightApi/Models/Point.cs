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

            var split = departure.Coordinates.Split(',');
            Latitude = double.Parse(split[1].Trim(), CultureInfo.InvariantCulture);
            Longitude = double.Parse(split[0].Trim(), CultureInfo.InvariantCulture);
        }

        public Point(string name, double latitude, double longitude)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }

        public string Icao { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
