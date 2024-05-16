using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Jekyll;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class JekyllCommand
    {
        private HardcodedConfig config;

        public JekyllCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(JekyllOptions options)
        {
            Console.WriteLine("Generating output for Jekyll.");
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];

            var jekyllExporter = new JekyllExporter(config.JekyllFolder, config.OurAirportFile);

            Console.WriteLine("Generating combined output.");
            jekyllExporter.ExportCombined(config.FlightsFolder);

            Console.WriteLine("Generating per flight output.");
            Parallel.ForEach(folders, folder =>
            {
                Console.Write($"\t {folder}");

                try
                {
                    jekyllExporter.ExportFlight(folder);
                    Console.WriteLine();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($" - skipping, files missing");
                }
            });
        }
    }
}
