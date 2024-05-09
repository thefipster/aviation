using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class NotamCommand
    {
        private HardcodedConfig config;

        public NotamCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(NotamOptions _)
        {
            Console.WriteLine("Extracting notams from simbrief xml exports into notam json format.");
            var folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);

            foreach (var folder in folders)
            {
                Console.Write($"\t {folder}");
                var files = new FlightFileScanner().GetFiles(folder);
                if (!files.Values.Any(x => x == FileTypes.SimbriefXml))
                {
                    Console.WriteLine(" - skipping, no simbrief xml file.");
                    continue;
                }

                var xmlFile = files.First(x => x.Value == Domain.Enums.FileTypes.SimbriefXml);
                var data = new SimbriefXmlLoader().ReadNotams(xmlFile.Key);
                data.FileType = Domain.Enums.FileTypes.NotamJson;
                new JsonWriter<NotamReport>().Write(folder, data, FileTypes.NotamJson, data.Departure, data.Arrival, true);
                Console.WriteLine(" - converted");
            }
        }
    }
}
