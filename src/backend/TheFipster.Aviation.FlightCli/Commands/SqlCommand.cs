using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
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
            var result = new SimToolkitProSqlReader().Read(config.SimToolkitProDatabaseFile, options.DepartureAirport, options.ArrivalAirport);
            result.FileType = FileTypes.SimToolkitProJson;
            var flightFolder = new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport);
            new JsonWriter<SimToolkitProFlight>().Write(flightFolder, result, FileTypes.SimToolkitProJson, options.DepartureAirport, options.ArrivalAirport);
        }
    }
}
