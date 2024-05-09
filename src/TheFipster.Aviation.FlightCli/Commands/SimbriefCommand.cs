using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class SimbriefCommand
    {
        private readonly HardcodedConfig config;

        public SimbriefCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(SimbriefOptions _)
        {
            Console.WriteLine("Converting SimBrief export:");
            var folders = new FileSystemFinder().GetFlightFolders(config.FlightsFolder);

            foreach (var folder in folders)
            {
                new SimbriefImporter().Import(folder);
                Console.WriteLine($"\t {folder}");
            }
        }
    }
}
