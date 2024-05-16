using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class NaviCommand
    {
        private HardcodedConfig config;

        public NaviCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(NaviOptions options)
        {
            var flightPath = new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport);
            Console.WriteLine($"Print the used charts from Navigraph as pdf into the folder {config.NavigraphFolder}");
            Console.WriteLine("When you're ready press ENTER.");
            Console.ReadLine();
            Console.WriteLine($"Moving charts: {config.NavigraphFolder} --> {flightPath}");
            var files = new FileOperations().MoveFiles(config.NavigraphFolder, flightPath);

            foreach (var file in files)
                Console.WriteLine($"\t {Path.GetFileName(file)}");
        }
    }
}
