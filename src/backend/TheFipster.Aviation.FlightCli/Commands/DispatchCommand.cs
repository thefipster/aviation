using System.Diagnostics;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class DispatchCommand
    {
        private HardcodedConfig config;

        public DispatchCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(DispatchOptions options)
        {
            Console.WriteLine("Dispatch your flight with SimBrief. When the files are synced this continues.");
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
