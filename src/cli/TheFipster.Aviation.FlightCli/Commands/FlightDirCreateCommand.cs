using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Creates the flight folder.
    /// </summary>
    public class FlightDirCreateCommand : IFlightRequiredCommand<FlightDirCreateOptions>
    {
        public void Run(FlightDirCreateOptions options, IConfig config)
        {
            if (config == null)
                throw new MissingConfigException("No config available.");

            var flightPath = new FileOperations()
                .CreateFlightFolder(
                config.FlightPlanFile,
                config.FlightsFolder,
                options.DepartureAirport,
                options.ArrivalAirport);

            Console.WriteLine($"Created flight folder at {flightPath}.");
        }
    }
}
