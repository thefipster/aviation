using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.SimToolkitPro;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class ToolkitCommand
    {
        private HardcodedConfig config;
        private FileSystemFinder finder;

        public ToolkitCommand(HardcodedConfig config)
        {
            this.config = config;
            this.finder = new FileSystemFinder();
        }

        internal void Run(ToolkitOptions options)
        {
            Console.WriteLine("Splitting all SimToolkitPro files into logbook and landing.");
            var flights = finder.GetFlightFolders(config.FlightsFolder);
            foreach (var flight in flights)
            {
                var departure = finder.GetDeparture(flight);
                var arrival = finder.GetArrival(flight);

                new SimToolkitProImporter().Import(flight, config.SimToolkitProDatabaseFile, departure, arrival);
                Console.WriteLine($"\t {flight}");
            }
        }
    }
}
