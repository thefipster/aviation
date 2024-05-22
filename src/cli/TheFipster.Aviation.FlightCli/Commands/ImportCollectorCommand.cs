using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.SimToolkitPro.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class ImportCollectorCommand : IFlightRequiredCommand<ImportCollectorOptions>
    {
        private readonly SimToolkitProSqlReader stkpReader;
        private readonly JsonWriter<SimToolkitProFlight> stkpWriter;
        private readonly FileOperations fileOperations;

        public ImportCollectorCommand()
        {
            stkpReader = new SimToolkitProSqlReader();
            stkpWriter = new JsonWriter<SimToolkitProFlight>();
            fileOperations = new FileOperations();
        }

        public void Run(ImportCollectorOptions options, IConfig config)
        {
            Console.WriteLine(ImportCollectorOptions.Welcome);

            var folder = options.GetFlightFolder(config.FlightsFolder);

            // simtoolkitpro
            var stkpFlight = stkpReader.Read(config.SimToolkitProDatabaseFile, options.DepartureAirport, options.ArrivalAirport);
            stkpWriter.Write(folder, stkpFlight, FileTypes.SimToolkitProJson, stkpFlight.Logbook.Dep, stkpFlight.Logbook.Arr);

            // navigraph charts
            var chartFiles = fileOperations.CollectFiles(config.NavigraphFolder, "*.pdf");
            fileOperations.MoveFiles(chartFiles, folder);

            // screenshots
            var imageFiles = fileOperations.CollectFiles(config.ScreenshotFolder, "*.png");
            fileOperations.MoveScreenshots(imageFiles, folder, options.DepartureAirport, options.ArrivalAirport);
        }
    }
}
