using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thefipster.Aviation.Modules.Screenshots.Components;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class CropCommand
    {
        private HardcodedConfig config;

        public CropCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(CropOptions options)
        {
            Console.WriteLine("Cropping title bar.");
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];


            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");

                var screenshots = new FlightFileScanner().GetFiles(folder, FileTypes.Screenshot);
                foreach (var screenshot in screenshots)
                    new ImageTitleCropper().CropTitle(screenshot, true);
            }
        }
    }
}
