using Thefipster.Aviation.Modules.Screenshots.Components;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class CreatePreviewForImagesCommand : IFlightCommand<CreatePreviewForImagesOptions>
    {
        public void Run(CreatePreviewForImagesOptions options, IConfig config)
        {
            Console.WriteLine("Resizing the screenshots for previewing.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");

                var screenshots = new FlightFileScanner().GetFiles(folder, FileTypes.Screenshot);
                Parallel.ForEach(screenshots, screenshot =>
                {
                    Console.WriteLine($"\t\t {Path.GetFileName(screenshot)}");
                    new ImageResizer().Resize(screenshot, options.Width, options.Height, true);
                });

                var charts = new FlightFileScanner().GetFiles(folder, FileTypes.ChartPrint);
                Parallel.ForEach(charts, chart =>
                {
                    Console.WriteLine($"\t\t {Path.GetFileName(chart)}");
                    new ImageResizer().Resize(chart, options.Width, options.Height, true);
                });
            }
        }
    }
}
