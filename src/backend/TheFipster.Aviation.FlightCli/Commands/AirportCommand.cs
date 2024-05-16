using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Airports.Components;
using TheFipster.Aviation.Modules.Simbrief.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class AirportCommand
    {
        private HardcodedConfig config;

        public AirportCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(AirportOptions options)
        {
            Console.WriteLine("Creating airport files for departure, arrival and alternate.");
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];

            var reader = new JsonReader<IEnumerable<Airport>>();
            var airports = new AirportFinder(reader, config.AirportFile);

            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");
                var file = new FlightFileScanner().GetFile(folder, FileTypes.SimbriefXml);
                var flight = new SimbriefXmlLoader().Read(file);

                writeAirport(airports, folder, flight.Departure.Icao);
                writeAirport(airports, folder, flight.Arrival.Icao);
                writeAirport(airports, folder, flight.Alternate.Icao);
            }
        }

        private static void writeAirport(AirportFinder airports, string folder, string icao)
        {
            var airport = airports.SearchWithIcao(icao);
            new JsonWriter<Airport>().Write(folder, airport, FileTypes.AirportJson, airport.Ident);
        }
    }
}
