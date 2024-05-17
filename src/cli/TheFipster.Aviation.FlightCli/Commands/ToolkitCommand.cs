using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.SimToolkitPro;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class ToolkitCommand
    {
        internal void Run(ToolkitOptions options, IConfig config)
        {
            Console.WriteLine("Reading STKP database file and exporting track, logbook and landing.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");
                new SimToolkitProImporter().Import(folder, config.SimToolkitProDatabaseFile, options.DepartureAirport, options.ArrivalAirport);
            }
        }
    }
}
