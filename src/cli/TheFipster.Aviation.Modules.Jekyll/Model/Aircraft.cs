using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.OurAirports;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    internal class Aircraft
    {
        public Aircraft()
        {
              
        }

        public Aircraft(int flightCount, Stats totalStats, OurAirport lastAirport, List<string> visitedCountries)
        {
            Flights = flightCount;
            Stats = totalStats;
            VisitedCountries = visitedCountries;
            Airport = lastAirport;
        }

        public OurAirport Airport { get; set; }
        public Stats Stats { get; set; }
        public List<string> VisitedCountries { get; set; }
        public int Flights { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
