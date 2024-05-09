using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Domain
{
    public class SimbriefWaypoints : JsonBase
    {
        public SimbriefWaypoints()
        {
            Waypoints = new List<Waypoint>();
        }

        public SimbriefWaypoints(string departure, string arrival)
            : this()
        {
            Departure = departure;
            Arrival = arrival;
        }

        public string? Departure { get; set; }
        public string? Arrival { get; set; }
        public ICollection<Waypoint> Waypoints { get; set; }
    }
}
