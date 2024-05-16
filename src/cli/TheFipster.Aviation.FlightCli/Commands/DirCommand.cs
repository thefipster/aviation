using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Creates the flight folder.
    /// </summary>
    public class DirCommand : ICommandRequired<DirOptions>
    {
        private HardcodedConfig config;

        public DirCommand() { }

        public DirCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        public void Run(DirOptions options, HardcodedConfig anotherConfig = null)
        {
            config = anotherConfig;

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
