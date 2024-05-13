using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
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

                string blackboxFile = null;
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

                var lines = new List<string>
                {
                    "Index;State"
                };
                for (int i = coordinates.Count() - 2; i > 0; i--)
                {

                    var bearing1 = GpsCalculator.GetBearing(coordinates[i-1].LatitudeDecimals, coordinates[i-1].LongitudeDecimals, coordinates[i].LatitudeDecimals, coordinates[i].LongitudeDecimals);
                    var bearing2 = GpsCalculator.GetBearing(coordinates[i].LatitudeDecimals, coordinates[i].LongitudeDecimals, coordinates[i+1].LatitudeDecimals, coordinates[i + 1].LongitudeDecimals);
                    var angle = Math.Abs(bearing1 - bearing2);

                    var distance = GpsCalculator.GetHaversineDistance(coordinates[i].LatitudeDecimals, coordinates[i].LongitudeDecimals, coordinates[i + 1].LatitudeDecimals, coordinates[i + 1].LongitudeDecimals);
                    if (angle < 10 && distance < 10)
                    {
                        coordinates.RemoveAt(i);
                    }
                    else
                    {
                        lines.Add($"{i:0000};{angle:000.0};{distance:0.000}");
                    }
                }

                var compressedBlackbox = new BlackBoxFlight(blackbox.Origin, blackbox.Destination);
                compressedBlackbox.Records = coordinates;
                new JsonWriter<BlackBoxFlight>().Write(folder, compressedBlackbox, FileTypes.BlackBoxCompressedJson, compressedBlackbox.Origin, compressedBlackbox.Destination, true);
                File.WriteAllLines("E:\\aviation\\Data\\Temp\\BlackBoxOptimizer.csv", lines);
                Console.WriteLine($" - {blackbox.Records.Count} --> {compressedBlackbox.Records.Count}");
            }
        }
    }
}
