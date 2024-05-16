using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Takes the blackbox trimmed file and compresses it as much as possible keeping the gps track as accurate as possible and outputting a blackbox compressed file.
    /// </summary>
    internal class CompressCommand
    {
        private HardcodedConfig config;

        public CompressCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(CompressOptions options)
        {
            Console.WriteLine("Compressing track of blackbox.");
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];


            foreach (var folder in folders)
            {
                Console.Write($"\t {folder}");

                string blackboxFile;
                try
                {
                    blackboxFile = new FlightFileScanner().GetFile(folder, FileTypes.BlackBoxTrimmedJson);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($" - skipping, no blackbox file.");
                    continue;
                }

                var blackbox = new JsonReader<BlackBoxFlight>().FromFile(blackboxFile);
                var coordinates = blackbox.Records.ToList();

                for (int i = coordinates.Count() - 2; i >= 0; i--)
                {
                    if (coordinates[i].Equals(coordinates[i + 1]))
                    {
                        coordinates.RemoveAt(i + 1);
                    }
                }

                for (int i = coordinates.Count() - 2; i > 0; i--)
                {

                    var bearing1 = GpsCalculator.GetBearing(coordinates[i - 1].LatitudeDecimals, coordinates[i - 1].LongitudeDecimals, coordinates[i].LatitudeDecimals, coordinates[i].LongitudeDecimals);
                    var bearing2 = GpsCalculator.GetBearing(coordinates[i].LatitudeDecimals, coordinates[i].LongitudeDecimals, coordinates[i + 1].LatitudeDecimals, coordinates[i + 1].LongitudeDecimals);
                    var angle = Math.Abs(bearing1 - bearing2);

                    var distance = GpsCalculator.GetHaversineDistance(coordinates[i].LatitudeDecimals, coordinates[i].LongitudeDecimals, coordinates[i + 1].LatitudeDecimals, coordinates[i + 1].LongitudeDecimals);
                    if (angle < 10 && distance < 10)
                        coordinates.RemoveAt(i);
                }

                var compressedBlackbox = new BlackBoxFlight(blackbox.Origin, blackbox.Destination);
                compressedBlackbox.Records = coordinates;
                new JsonWriter<BlackBoxFlight>().Write(folder, compressedBlackbox, FileTypes.BlackBoxCompressedJson, compressedBlackbox.Origin, compressedBlackbox.Destination, true);
                Console.WriteLine($" - {blackbox.Records.Count} --> {compressedBlackbox.Records.Count}");
            }
        }
    }
}
