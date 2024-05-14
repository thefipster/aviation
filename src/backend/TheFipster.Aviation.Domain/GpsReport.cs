using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Domain
{
    public class GpsReport : JsonBase
    {
        public GpsReport() 
        {
            Coordinates = new List<Coordinate>();
            Waypoints = new List<Waypoint>();
            GeoTags = new List<GeoTag>();
            BlackBoxEvents = new List<Waypoint>();
        }

        public GpsReport(
            string? departureAirport, 
            string? arrivalAirport, 
            List<Coordinate> coordinates, 
            List<Waypoint> waypoints, 
            List<GeoTag>? geoTags, 
            List<Waypoint>? blackBoxEvents)
        {
            Departure = departureAirport;
            Arrival = arrivalAirport;

            Coordinates = coordinates;
            Waypoints = waypoints;
            GeoTags = geoTags ?? new List<GeoTag>();
            BlackBoxEvents = blackBoxEvents ?? new List<Waypoint>();
        }

        public string? Departure { get; }
        public string? Arrival { get; }
        public List<Coordinate> Coordinates { get; }
        public List<Waypoint> Waypoints { get; }
        public List<GeoTag>? GeoTags { get; }
        public List<Waypoint>? BlackBoxEvents { get; }
    }
}
