using System.Diagnostics;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Moves the simbrief dispatch files downloaded by the Simbrief Downloader into the flight folder.
    /// </summary>
    public class SimbriefDispatchMoveCommand : ICommandRequired<SimbriefDispatchMoveOptions>
    {
        private HardcodedConfig config;

        public SimbriefDispatchMoveCommand() { }

        public SimbriefDispatchMoveCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        public void Run(SimbriefDispatchMoveOptions options, HardcodedConfig anotherConfig = null)
        {
            Console.WriteLine("Dispatch your flight with SimBrief. When the files are synced this continues.");

            config = anotherConfig;
            if (config == null)
                throw new MissingConfigException("No config available.");

            Process.Start("chrome.exe", "https://dispatch.simbrief.com/options/new");

            var simbriefFiles = new List<string>();
            while (simbriefFiles.Count == 0)
            {
                Thread.Sleep(1000);
                simbriefFiles = new SimbriefFinder().FindExportFiles(config.SimbriefFolder, options.DepartureAirport, options.ArrivalAirport).ToList();

                if (simbriefFiles.Count > 0)
                {
                    Console.WriteLine("Simbrief export detected. Waiting until download finished.");
                    Thread.Sleep(5000);
                }
            }

            var flightPath = new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport);
            Console.WriteLine($"Copying simbrief files to {flightPath}:");
            var searchPattern = $"{options.DepartureAirport}{options.ArrivalAirport}*";
            new FileOperations().MoveFiles(config.SimbriefFolder, flightPath, searchPattern);
        }
    }
}
