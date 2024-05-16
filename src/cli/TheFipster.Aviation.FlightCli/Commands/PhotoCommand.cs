using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class PhotoCommand
    {
        private HardcodedConfig config;

        public PhotoCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(PhotoOptions options)
        {
            var flightPath = new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport);
            Console.WriteLine($"Moving screenshots from {config.ScreenshotFolder} --> {flightPath}");
            var files = new FileOperations().MoveFiles(config.ScreenshotFolder, flightPath, "Microsoft Flight Simulator*.png");
            foreach (var file in files)
                Console.WriteLine($"\t {Path.GetFileName(file)}");
        }
    }
}
