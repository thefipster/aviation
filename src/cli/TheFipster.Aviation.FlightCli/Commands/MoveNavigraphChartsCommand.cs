using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class MoveNavigraphChartsCommand : IFlightRequiredCommand<MoveNavigraphChartsOptions>
    {
        public void Run(MoveNavigraphChartsOptions options, IConfig config)
        {
            if (config == null)
                throw new MissingConfigException("No config available.");

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
