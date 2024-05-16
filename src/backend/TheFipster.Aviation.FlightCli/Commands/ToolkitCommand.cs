using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.SimToolkitPro;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class ToolkitCommand
    {
        private HardcodedConfig config;

        public ToolkitCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(ToolkitOptions options)
        {
            Console.WriteLine("Reading STKP database file and exporting track, logbook and landing.");
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];

            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");
                new SimToolkitProImporter().Import(folder, config.SimToolkitProDatabaseFile, options.DepartureAirport, options.ArrivalAirport);
            }
        }
    }
}
