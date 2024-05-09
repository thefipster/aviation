using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Options;
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
                var files = new FlightFileScanner().GetFiles(folder);
                if (!files.ContainsValue(Domain.Enums.FileTypes.SimbriefXml))
                {
                    Console.WriteLine($"\t skipping {folder} - no simbrief xml.");
                    continue;
                }

                var simbriefXml = files.First(x => x.Value == Domain.Enums.FileTypes.SimbriefXml);
                var flight = new SimbriefXmlLoader().Read(simbriefXml.Key);

                if (flight.Arrival.Icao == null)
                {
                    Console.WriteLine($"\t skipping {folder} - simbiref missing arrival.");
                    continue;
                }
                if (flight.Departure.Icao == null)
                {
                    Console.WriteLine($"\t skipping {folder} - simbiref missing departure.");
                    continue;
                }
                if (flight.Alternate.Icao == null)
                {
                    Console.WriteLine($"\t skipping {folder} - simbiref missing alternate.");
                    continue;
                }

                var arrivalIcao = flight.Arrival.Icao;
                var arrival = new Modules.Airports.AirportFinder(config.AirportFile).SearchWithIcao(arrivalIcao);
                if (arrival == null)
                {
                    Console.WriteLine($"\t skipping {folder} - arrival airport missing.");
                    continue;
                }
                arrival.FileType = Domain.Enums.FileTypes.AirportJson;
                new JsonWriter<Airport>().Write(folder, arrival, FileTypes.AirportJson, arrivalIcao, null, true);

                var departureIcao = flight.Departure.Icao;
                var departure = new Modules.Airports.AirportFinder(config.AirportFile).SearchWithIcao(departureIcao);
                if (departure == null)
                {
                    Console.WriteLine($"\t skipping {folder} - departure airport missing.");
                    continue;
                }
                departure.FileType = Domain.Enums.FileTypes.AirportJson;
                new JsonWriter<Airport>().Write(folder, departure, FileTypes.AirportJson, departureIcao, null, true);

                var alternateIcao = flight.Alternate.Icao;
                var alternate = new Modules.Airports.AirportFinder(config.AirportFile).SearchWithIcao(alternateIcao);
                if (alternate == null)
                {
                    Console.WriteLine($"\t skipping {folder} - alternate airport missing.");
                    continue;
                }
                alternate.FileType = Domain.Enums.FileTypes.AirportJson;
                new JsonWriter<Airport>().Write(folder, alternate, FileTypes.AirportJson, alternateIcao, null, true);

                Console.WriteLine($"\t checked {folder} - {departureIcao}/{arrivalIcao}/{alternateIcao}");
            }
        }
    }
}
