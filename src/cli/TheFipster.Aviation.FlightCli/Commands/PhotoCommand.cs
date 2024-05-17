using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class PhotoCommand
    {
        internal void Run(PhotoOptions options, IConfig config)
        {
            if (config == null)
                throw new MissingConfigException("No config available.");

            var flightPath = new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport);
            Console.WriteLine($"Moving screenshots from {config.ScreenshotFolder} --> {flightPath}");

            var files = new FileOperations().MoveFiles(config.ScreenshotFolder, flightPath, "Microsoft Flight Simulator*.png");
            foreach (var file in files)
                Console.WriteLine($"\t {Path.GetFileName(file)}");
        }
    }
}
