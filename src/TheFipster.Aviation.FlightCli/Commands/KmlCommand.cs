using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class KmlCommand
    {
        private HardcodedConfig config;

        public KmlCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(KmlOptions options)
        {
            Console.Write($"Extracting route from simbrief kml.");
            var flights = new FileSystemFinder().GetFlightFolders(config.FlightsFolder);
            foreach (var flight in flights)
            {
                var route = new SimbriefKmlLoader().Load(flight);
                new JsonWriter<Route>().Write(flight, route, Domain.Enums.FileTypes.RouteJson, route.Departure, route.Arrival);
                Console.WriteLine($"\t {flight}.");
            }
        }
    }
}
