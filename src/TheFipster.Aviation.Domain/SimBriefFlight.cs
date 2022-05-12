using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Domain
{
    public class SimBriefFlight
    {
        public SimBriefFlight()
        {
            Waypoints = new List<Waypoint>();
        }

        public string AiracCycle { get; set; }
        public long DispatchDate { get; set; }
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
        public int CostIndex { get; set; }
        public int Altitude { get; set; }
        public int WindComponent { get; set; }
        public int GreatCircleDistance { get; set; }
        public int RouteDistance { get; set; }
        public int AirDistance { get; set; }
        public int Passengers { get; set; }
        public string Route { get; set; }

        public Airport Departure { get; set; }
        public Airport Arrival { get; set; }
        public Airport Alternate { get; set; }

        public ICollection<Waypoint> Waypoints { get; set; }
    }
}
