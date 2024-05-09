using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.SimToolkitPro;
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

            if (exportData == null || exportData.Landings == null)
            {
                Console.WriteLine("couldn't load export file.");
                return;
            }

            Console.WriteLine($" using latest export file: {Path.GetFileName(latestExportFilepath)}");
            Console.WriteLine();

            foreach (var landing in exportData.Landings)
            {
                var flight = exportData.Logbook.FirstOrDefault(x => x.LocalId == landing.FlightId);

                if (flight == null)
                    continue;

                var departure = flight.Dep;
                var arrival = flight.Arr;

                try
                {
                    var flightFolder = new FileSystemFinder().GetFlightFolder(config.FlightsFolder, departure, arrival);
                    landing.FileType = FileTypes.LandingJson;
                    new JsonWriter<Landing>().Write(flightFolder, landing, FileTypes.LandingJson, departure, arrival, true);
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
