using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Jekyll;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Takes every file it needs to generate the output files for jekyll.
    /// </summary>
    internal class JekyllCommand : IFlightCommand<JekyllOptions>
    {
        public void Run(JekyllOptions options, IConfig config)
        {
            Console.WriteLine("Generating output for Jekyll.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            var jekyllExporter = new JekyllExporter(config.JekyllFolder, config.OurAirportFile);

            Console.WriteLine("Generating combined output.");
            jekyllExporter.ExportCombined(config.FlightsFolder);

            Console.WriteLine("Generating per flight output.");
            Parallel.ForEach(folders, folder =>
            {

                try
                {
                    jekyllExporter.ExportFlight(folder);
                    Console.WriteLine($"\t {folder}");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($"\t {folder} - skipping, files missing");
                }
            });
        }
    }
}
