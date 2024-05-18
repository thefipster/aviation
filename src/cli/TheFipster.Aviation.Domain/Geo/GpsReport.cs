using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Domain.Geo
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

        public string? Departure { get; set; }
        public string? Arrival { get; set; }
        public ICollection<Coordinate> Coordinates { get; set; }
        public ICollection<Waypoint> Waypoints { get; set; }
        public ICollection<GeoTag>? GeoTags { get; set; }
        public ICollection<Waypoint>? BlackBoxEvents { get; set; }
    }
}
