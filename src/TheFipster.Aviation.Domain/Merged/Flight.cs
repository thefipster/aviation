using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.Domain.Merged
{
    public class Flight
    {
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
        public string AiracCycle { get; set; }
        public int AirDistance { get; set; }
        public int GreatCircleDistance { get; set; }
        public int RouteDistance { get; set; }
        public string Route { get; set; }
        public int Altitude { get; set; }
        public int CostIndex { get; set; }
        public long DispatchedOn { get; set; }
        public int WindComponent { get; set; }
        public int Passengers { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string Alternate { get; set; }
        public long DepartedOn { get; set; }
        public long ArrivedOn { get; set; }
        public string FuelRamp { get; set; }
        public string FuelShutdown { get; set; }
        public string Aircraft { get; set; }
        public List<Airport> Airports { get; set; }
        public Landing Landing { get; set; }
        public IEnumerable<Record> Blackbox { get; set; }
        public IEnumerable<Waypoint> Waypoints { get; set; }
    }
}
