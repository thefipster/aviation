using Thefipster.Aviation.Modules.Screenshots.Components;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class PreviewCommand
    {
        private HardcodedConfig config;
        private readonly ImageResizer resizer;

        public PreviewCommand(HardcodedConfig config)
        {
            this.config = config;
            resizer = new ImageResizer();
        }

        internal void Run(PreviewOptions options)
        {
            Console.WriteLine("Resizing the screenshots for previewing.");
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];


            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");

                var files = new FlightFileScanner().GetFiles(folder, FileTypes.Screenshot);
                foreach (var file in files)
                {
                    Console.WriteLine($"\t\t {Path.GetFileName(file)}");
                    resizer.Resize(file, options.Height);
                }
            }
        }
    }
}
