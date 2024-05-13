using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class SimbriefCommand
    {
        private readonly HardcodedConfig config;

        public SimbriefCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(SimbriefOptions options)
        {
            Console.WriteLine("Converting Simbrief export files into Flight, Waypoints, Notams, Route and OFP.");
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];


            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");
                new SimbriefImporter().Import(folder);
            }
        }
    }
}
