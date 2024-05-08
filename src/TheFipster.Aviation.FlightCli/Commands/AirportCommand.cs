using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Airports;
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
            Console.WriteLine("Creating all missing airport files in the flight folders.");
            var flightFolders = Directory.GetDirectories(config.FlightsFolder);
            foreach (var folder in flightFolders)
            {
                var files = new ScanCommand(config).Scan(folder);
                if (!files.ContainsValue(Domain.Enums.FileTypes.SimbriefXml))
                {
                    Console.WriteLine($"\t skipping {folder} - no simbrief xml.");
                    continue;
                }

                var simbriefXml = files.First(x => x.Value == Domain.Enums.FileTypes.SimbriefXml);
                var flight = new Loader().Read(simbriefXml.Key);

                var arrivalIcao = flight.Arrival.Icao;
                var arrival = new Modules.Airports.Finder(config.AirportFile).SearchWithIcao(arrivalIcao);
                new JsonWriter<Airport>().Write(folder, arrival, "Airport", arrivalIcao);

                var departureIcao = flight.Departure.Icao;
                var departure = new Modules.Airports.Finder(config.AirportFile).SearchWithIcao(departureIcao);
                new JsonWriter<Airport>().Write(folder, departure, "Airport", departureIcao);

                var alternateIcao = flight.Alternate.Icao;
                var alternate = new Modules.Airports.Finder(config.AirportFile).SearchWithIcao(alternateIcao);
                new JsonWriter<Airport>().Write(folder, alternate, "Airport", alternateIcao);

                Console.WriteLine($"\t checked {folder} - {departureIcao}/{arrivalIcao}/{alternateIcao}");
            }
        }
    }
}
