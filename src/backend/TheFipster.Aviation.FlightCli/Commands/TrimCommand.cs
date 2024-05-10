using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.BlackBox;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class TrimCommand
    {
        private HardcodedConfig config;

        public TrimCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(TrimOptions options)
        {
            Console.WriteLine("Trimming the black box files to contain only engine on section.");
            var folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);

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
