using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Modules.FlightPlan.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Modules.Airports.Abstractions;
using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.Modules.FlightPlan.Components
{
    public class OperationsPlan : IOperationsPlan
    {
        private readonly IFlightPlanReader flightPlan;
        private readonly IFlightFinder flights;
        private readonly IFlightMeta meta;
        private readonly IAirportFinder airports;
        private readonly IFlightFileScanner scanner;
        private readonly IJsonReader<Stats> statsReader;

        public OperationsPlan(
            IFlightPlanReader flightPlan, 
            IFlightFinder flights, 
            IFlightMeta meta, 
            IAirportFinder airports, 
            IFlightFileScanner scanner,
            IJsonReader<Stats> statsReader)
        {
            this.flightPlan = flightPlan;
            this.flights = flights;
            this.meta = meta;
            this.airports = airports;
            this.scanner = scanner;
            this.statsReader = statsReader;
        }

        public PlannedFlight GetNextFlight()
        {
            var flightplan = flightPlan.GetFlightPlan();
            var latestFlight = flights.GetLatestFlight();

            var from = meta.GetDeparture(latestFlight);
            var to = meta.GetArrival(latestFlight);

            var lastLeg = flightplan.FirstOrDefault(x => x.From == from && x.To == to);
            var nextLeg = flightplan.FirstOrDefault(x => x.No == lastLeg.No + 1);

            var departure = airports.SearchWithIcao(nextLeg.From);
            var arrival = airports.SearchWithIcao(nextLeg.To);
            var nextFlight = new PlannedFlight(nextLeg.No, departure, arrival);

            return nextFlight;
        }

        public Stats GetLastFlight()
        {
            var latestFlightPath = flights.GetLatestFlight();
            var statsFile = scanner.GetFile(latestFlightPath, FileTypes.StatsJson);
            var stats = statsReader.FromFile(statsFile);

            return stats;
        }
    }
}
