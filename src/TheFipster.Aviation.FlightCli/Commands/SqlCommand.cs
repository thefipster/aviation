using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.SimToolkitPro.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class SqlCommand
    {
        private HardcodedConfig config;

        public SqlCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(SqlOptions options)
        {
            Console.WriteLine("Reading STKP database file");
            var result = new SqlReader().Read(config.SimToolkitProDatabaseFile, options.DepartureAirport, options.ArrivalAirport);
            result.FileType = Domain.Enums.FileTypes.SimToolkitProJson;
            var flightFolder = new FileSystemFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport);
            new JsonWriter<SimToolkitProFlight>().Write(flightFolder, result, "SimToolkitPro", options.DepartureAirport, options.ArrivalAirport);
        }
    }
}
