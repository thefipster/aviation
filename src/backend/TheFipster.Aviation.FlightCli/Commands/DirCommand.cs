using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class DirCommand
    {
        private HardcodedConfig config;

        public DirCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(DirOptions options)
        {
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
