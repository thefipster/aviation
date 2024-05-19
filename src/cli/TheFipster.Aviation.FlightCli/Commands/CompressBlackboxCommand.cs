using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.BlackBox;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Takes the blackbox trimmed file and compresses it as much as possible keeping the gps track as accurate as possible and outputting a blackbox compressed file.
    /// </summary>
    public class CompressBlackboxCommand : IFlightCommand<CompressBlackboxOptions>
    {
        public void Run(CompressBlackboxOptions options, IConfig config)
        {
            Console.WriteLine("Compressing track of blackbox.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            foreach (var folder in folders)
            {
                Console.Write($"\t {folder}");

                try
                {
                    BlackBoxFlight compressedBlackbox = new BlackBoxCompressor().CompressBlackboxRecords(folder);
                    new JsonWriter<BlackBoxFlight>().Write(folder, compressedBlackbox, FileTypes.BlackBoxCompressedJson, compressedBlackbox.Origin, compressedBlackbox.Destination, true);
                    Console.WriteLine();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($" - skipping, no blackbox file.");
                    continue;
                }
            }
        }
    }
}
