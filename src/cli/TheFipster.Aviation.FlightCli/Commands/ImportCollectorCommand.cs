using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.SimToolkitPro;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class ImportCollectorCommand : IFlightRequiredCommand<ImportCollectorOptions>
    {
        private readonly FileOperations fileOperations;
        private readonly StkpOps stkpOps;

        public ImportCollectorCommand()
        {
            fileOperations = new FileOperations();
            stkpOps = new StkpOps();
        }

        public void Run(ImportCollectorOptions options, IConfig config)
        {
            Console.WriteLine(ImportCollectorOptions.Welcome);

            var folder = options.GetFlightFolder(config.FlightsFolder);

            // simtoolkitpro
            var stkpFlight = stkpOps.ReadFlight(config.SimToolkitProDatabaseFile, options.DepartureAirport, options.ArrivalAirport);
            stkpOps.WriteFlight(folder, stkpFlight);

            // navigraph charts
            var chartFiles = fileOperations.CollectFiles(config.NavigraphFolder, "*.pdf");
            fileOperations.MoveFiles(chartFiles, folder);

            // screenshots
            var imageFiles = fileOperations.CollectFiles(config.ScreenshotFolder, "*.png");
            fileOperations.MoveScreenshots(imageFiles, folder, options.DepartureAirport, options.ArrivalAirport);
        }
    }
}
