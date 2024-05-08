using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.SimToolkitPro.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class LandingCommand
    {
        private HardcodedConfig config;

        public LandingCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(LandingOptions o)
        {
            Console.Write($"Scanning SimToolkitPro Export Folder for landings");
            var latestExportFilepath = new Finder().Find(config.SimToolkitProFolder);
            var exportData = new JsonReader<SimToolkitProExport>().FromFile(latestExportFilepath);
            Console.WriteLine($" using latest export file: {Path.GetFileName(latestExportFilepath)}");
            Console.WriteLine();

            foreach (var landing in exportData.Landings)
            {
                var aircraft = exportData.Fleet.FirstOrDefault(x => x.LocalId == landing.FleetId);
                var flight = exportData.Logbook.FirstOrDefault(x => x.LocalId == landing.FlightId);

                if (flight == null)
                    continue;

                var departure = flight.Dep;
                var arrival = flight.Arr;

                try
                {
                    var flightFolder = new FileSystemFinder().GetFlightFolder(config.FlightsFolder, departure, arrival);
                    var report = new LandingReport(landing, flight, aircraft);
                    new JsonWriter<LandingReport>().Write(flightFolder, report, "Landing", departure, arrival);
                    Console.WriteLine($"\t matched {departure} - {arrival}.");
                }
                catch (Exception)
                {
                    Console.WriteLine($"\t no match {departure} - {arrival}.");
                }
            }
        }
    }
}
