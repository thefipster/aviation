using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    internal class Aircraft
    {
        public Aircraft(int flightCount, Stats totalStats, OurAirport lastAirport, Coordinate lastPosition, List<string> visitedCountries)
        {
            Flights = flightCount;
            Stats = totalStats;
            VisitedCountries = visitedCountries;
            Airport = lastAirport;
            Position = lastPosition;
        }

        public OurAirport Airport { get; set; }
        public Coordinate Position { get; }
        public Stats Stats { get; set; }
        public List<string> VisitedCountries { get; set; }
        public int Flights { get; set; }
    }
}
