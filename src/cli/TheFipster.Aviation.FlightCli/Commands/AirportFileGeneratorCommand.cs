using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Airports.Components;
using TheFipster.Aviation.Modules.Simbrief.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Creates airport files from DataHub source for a flight
    /// </summary>
    public class AirportFileGeneratorCommand : IFlightCommand<AirportFileGeneratorOptions>
    {
        private IConfig config;

        public AirportFileGeneratorCommand() { }

        public AirportFileGeneratorCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        public void Run(AirportFileGeneratorOptions options, IConfig anotherConfig = null)
        {
            Console.WriteLine("Creating airport files for departure, arrival and alternate.");

            config = anotherConfig;
            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = getFolders(options);
            var airports = getAirports();

            foreach (var folder in folders)
                writeAirportsForFolder(airports, folder);
        }

        private IEnumerable<string> getFolders(AirportFileGeneratorOptions options)
        {
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];
            return folders;
        }

        private AirportFinder getAirports()
        {
            var reader = new JsonReader<IEnumerable<Airport>>();
            var airports = new AirportFinder(reader, config.AirportFile);
            return airports;
        }

        private static void writeAirportsForFolder(AirportFinder airports, string folder)
        {
            Console.WriteLine($"\t {folder}");
            var file = new FlightFileScanner().GetFile(folder, FileTypes.SimbriefXml);
            var flight = new SimbriefXmlLoader().Read(file);

            writeAirport(airports, folder, flight.Departure.Icao);
            writeAirport(airports, folder, flight.Arrival.Icao);
            writeAirport(airports, folder, flight.Alternate.Icao);
        }

        private static void writeAirport(AirportFinder airports, string folder, string icao)
        {
            var airport = airports.SearchWithIcao(icao);
            new JsonWriter<Airport>().Write(folder, airport, FileTypes.AirportJson, airport.Ident);
        }
    }
}
