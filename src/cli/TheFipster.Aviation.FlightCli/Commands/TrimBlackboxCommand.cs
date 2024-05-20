using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.BlackBox;
using TheFipster.Aviation.Modules.BlackBox.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class TrimBlackboxCommand : IFlightCommand<TrimBlackboxOptions>
    {
        public void Run(TrimBlackboxOptions options, IConfig config)
        {
            Console.WriteLine("Trimming the black box files to contain only engine on section.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            foreach (var folder in folders)
            {
                Console.Write($"\t {folder}");
                try
                {
                    var file = new FlightFileScanner().GetFile(folder, FileTypes.BlackBoxJson);
                    var blackBox = new JsonReader<BlackBoxFlight>().FromFile(file);
                    var trimmedFlight = new BlackBoxTrimmer().Trim(blackBox);
                    new JsonWriter<BlackBoxFlight>().Write(folder, trimmedFlight, FileTypes.BlackBoxTrimmedJson, trimmedFlight.Origin, trimmedFlight.Destination);
                    Console.WriteLine($" - trimmed {blackBox.Records.Count()} --> {trimmedFlight.Records.Count()}.");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($" - skipping, no black box file.");
                    continue;
                }
            }
        }
    }
}
