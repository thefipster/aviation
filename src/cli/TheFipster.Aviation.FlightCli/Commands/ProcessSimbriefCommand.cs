using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class ProcessSimbriefCommand : IFlightCommand<ProcessSimbriefOptions>
    {
        public void Run(ProcessSimbriefOptions options, IConfig config)
        {
            Console.WriteLine("Converting Simbrief export files into Flight, Waypoints, Notams, Route and OFP.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");
                new SimbriefImporter().Import(folder);
            }
        }
    }
}
