
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain.Simbrief.Kml;

namespace TheFipster.Aviation.Domain.Simbrief
{
    public class Waypoint
    {
        public Waypoint() { }

        public Waypoint(OurAirport ourAirport)
        {
            Name = ourAirport.Ident;
            Latitude = ourAirport.Latitude;
            Longitude = ourAirport.Longitude;
        }

        public Waypoint(Placemark kmlPlacemark)
        {
            Name = kmlPlacemark.Name;

            var coordinate = Coordinate
                .FromAirportCoordinateString(kmlPlacemark.Point.Coordinates);

            Latitude = coordinate.Latitude;
            Longitude = coordinate.Longitude;
        }

        public Waypoint(int index, string airway, Datahub.Airport departure)
        {
            Index = index;
            Name = departure.Ident;
            Airway = airway;

            var coordinate = Coordinate
                .FromAirportCoordinateString(departure.Coordinates);

            Latitude = coordinate.Latitude;
            Longitude = coordinate.Longitude;
        }

        public Waypoint(string name, double lat, double lon)
        {
            Name = name;
            Latitude = lat;
            Longitude = lon;
        }

        public Waypoint(int index, string name, double lat, double lon)
            : this(name, lat, lon)
        {
            Index = index;
        }

        public int Index { get; set; }
        public string Name { get; set; }
        public string Airway { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
