using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Airports.Components;
using TheFipster.Aviation.Modules.FlightPlan.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class NextCommand
    {
        private readonly OperationsPlan plan;

        public NextCommand(HardcodedConfig config)
        {
            this.plan = new OperationsPlan(
                new FlightPlanReader(new JsonReader<IEnumerable<Leg>>(), config.FlightPlanFile),
                new FlightFinder(config.FlightsFolder),
                new FlightMeta(),
                new AirportFinder(new JsonReader<IEnumerable<Airport>>(), config.AirportFile),
                new FlightFileScanner(),
                new JsonReader<Stats>());
        }

        internal PlannedFlight Run()
        {
            var nextFlight = plan.GetNextFlight();

            Console.WriteLine("You're next flight will be:");
            Console.WriteLine($"{nextFlight.Leg}: {nextFlight.Departure.Ident} - {nextFlight.Arrival.Ident}");
            Console.WriteLine($"{nextFlight.Departure.Name} to {nextFlight.Arrival.Name}.");

            return nextFlight;
        }
    }
}
