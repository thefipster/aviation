﻿using TheFipster.Aviation.Domain.Exceptions;
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

            if (folders.Count() > 1)
            {
                Console.WriteLine("Generating combined output.");
                jekyllExporter.ExportCombined(config.FlightsFolder);
            }

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