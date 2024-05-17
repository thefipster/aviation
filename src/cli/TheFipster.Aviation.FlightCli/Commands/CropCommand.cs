using Thefipster.Aviation.Modules.Screenshots.Components;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Takes the screenshots preview files and cuts out the title bar. 
    /// For every crop operation a .cropped file is created to the file doesn't get cropped again should the command run again.
    /// A .cropnot file can also be created in a folder to exclude the it completely.
    /// </summary>
    public class CropCommand : IFlightCommand<CropOptions>
    {
        public void Run(CropOptions options, IConfig config)
        {
            Console.WriteLine("Cropping title bar.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
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
