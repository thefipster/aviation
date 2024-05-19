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
    /// Takes the blackbox file and searches for maximum values and flight events outputting the info into the blackbox stats file.
    /// </summary>
    public class ProcessBlackboxCommand : IFlightCommand<ProcessBlackboxOptions>
    {
        public void Run(ProcessBlackboxOptions options, IConfig config)
        {
            Console.WriteLine("Scans the blackbox for configuration events and record values.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            foreach (var folder in folders)
            {
                Console.Write($"\t {folder}");

                try
                {
                    var stats = new BlackBoxScanner().GenerateStatsFromBlackbox(folder);
                    new JsonWriter<BlackBoxStats>().Write(folder, stats, FileTypes.BlackBoxStatsJson, stats.Departure, stats.Arrival, true);
                    Console.WriteLine();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine(" - skipping, no blackbox file.");
                    continue;
                }
            }
        }
    }
}
