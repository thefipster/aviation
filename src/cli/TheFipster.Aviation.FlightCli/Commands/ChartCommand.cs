using Thefipster.Aviation.Modules.Screenshots.Components;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Uses the chart pdf files to generate png images.
    /// </summary>
    public class ChartCommand : IFlightCommand<ChartOptions>
    {
        public void Run(ChartOptions options, IConfig config)
        {
            Console.WriteLine("Converting chart pdfs into pngs.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");

                var charts = new FlightFileScanner().GetFiles(folder, Domain.Enums.FileTypes.Chart);
                foreach (var chart in charts)
                {
                    Console.WriteLine($"\t\t {Path.GetFileName(chart)}");
                    new PdfConverter().ToImage(chart);
                }
            }
        }
    }
}
