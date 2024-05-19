using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class WizardCommand : ICommand<WizardOptions>
    {
        private IConfig? config;

        public void Run(WizardOptions options, IConfig config)
        {
            Console.WriteLine("Hello Captain.");
            Console.WriteLine();

            this.config = config;
            if (config == null)
                throw new MissingConfigException("No config available.");

            var nextFlight = getNextFlight();
            var departure = nextFlight.Departure.Ident;
            var arrival = nextFlight.Arrival.Ident;

            if (string.IsNullOrWhiteSpace(departure) || string.IsNullOrWhiteSpace(arrival))
                throw new ApplicationException("Can't determine next flight.");

            // create flight folder
            runRequiredFlightCommand<FlightDirCreateCommand, FlightDirCreateOptions>(departure, arrival);
            // move simbrief import into flight folder -> use simbrief api to download instead of relying on simbrief downloader
            runRequiredFlightCommand<SimbriefDispatchMoveCommand, SimbriefDispatchMoveOptions>(departure, arrival);
            // create airport files into flight folder -> not realy useful, just use OurAirports source directly
            runGenericFlightCommand<AirportFileGeneratorCommand, AirportFileGeneratorOptions>(departure, arrival);
            // record black box
            runRequiredFlightCommand<BlackboxRecorderCommand, BlackboxRecorderOptions>(departure, arrival);
            // move navigraph charts -> since this seens to become an online blog this is useless due to copyright
            runRequiredFlightCommand<MoveNavigraphChartsCommand, MoveNavigraphChartsOptions>(departure, arrival);
            // move screenshots
            runRequiredFlightCommand<MoveScreenshotsCommand, MoveScreenshotsOptions>(departure, arrival);
            // import data from STKP sqlite database
            runGenericFlightCommand<ImportToolkitCommand, ImportToolkitOptions>(departure, arrival);

            // renames screnshots and simbrief import -> this should be done directly on simbrief and screenshot import
            runGenericFlightCommand<RenameImportFilesCommand, RenameImportFilesOptions>(departure, arrival);

            // use blackbox or simtoolkit to geocode screenshots
            runGenericFlightCommand<GeoTagScreenshotsCommand, GeoTagScreenshotsOptions>(departure, arrival);
            // processes simbrief import
            runGenericFlightCommand<ProcessSimbriefCommand, ProcessSimbriefOptions>(departure, arrival);
            // converts the chart pdfs into pngs
            runGenericFlightCommand<ConvertChartsToImageCommand, ConvertChartsToImageOptions>(departure, arrival);
            // converts all pngs into jpgs and preview jpgs
            runGenericFlightCommand<CreatePreviewForImagesCommand, CreatePreviewForImagesOptions>(departure, arrival);
            // trims the blackbox file into the black box trimmer file -> this is useless since the compress command can also do it but better.
            runGenericFlightCommand<TrimBlackboxCommand, TrimBlackboxOptions>(departure, arrival);
            // compresses the blackbox file to have as little coordinates as possible while preserving the track
            runGenericFlightCommand<CompressBlackboxCommand, CompressBlackboxOptions>(departure, arrival);
            // processes the blackbox and exports flight events and statistics
            runGenericFlightCommand<ProcessBlackboxCommand, ProcessBlackboxOptions>(departure, arrival);
            // combines all gps information of all sources into gps file
            runGenericFlightCommand<CombineGpsInformationCommand, CombineGpsInformationOptions>(departure, arrival);
            // combines all stats of all sources into stats file.
            runGenericFlightCommand<CombineStatsCommand, CombineStatsOptions>(departure, arrival);
            // compresses the track file to have as little coordinates as possible while preserving the track
            runGenericFlightCommand<CompressTrackCommand, CompressTrackOptions>(departure, arrival);
            // crops the screenshots to remove the title bar of the msfs2020 window 
            runGenericFlightCommand<CropScreenshotTitleCommand, CropScreenshotTitleOptions>(departure, arrival);
        }

        private void runGenericFlightCommand<Tc, To>(string departure, string arrival)
            where Tc : IFlightCommand<To>, new()
            where To : FlightOptions, new()
        {
            var command = new Tc();
            var options = new To();

            options.DepartureAirport = departure;
            options.ArrivalAirport = arrival;

            command.Run(options, new HardcodedConfig());
        }

        private void runRequiredFlightCommand<Tc, To>(string departure, string arrival)
            where Tc : IFlightRequiredCommand<To>, new()
            where To : FlightRequiredOptions, new()
        {
            var command = new Tc();
            var options = new To();

            options.DepartureAirport = departure;
            options.ArrivalAirport = arrival;

            command.Run(options, new HardcodedConfig());
        }

        private PlannedFlight getNextFlight()
        {
            var next = new NextCommand();
            var options = new NextOptions();
            return next.Run(options, config);
        }

        private void combineGps(string departure, string arrival)
        {
            var gpsCombiner = new CombineGpsInformationCommand();
            var options = new CombineGpsInformationOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            gpsCombiner.Run(options, config);
        }

        private void extractEvents(string departure, string arrival)
        {
            var eventExtractor = new ProcessBlackboxCommand();
            var options = new ProcessBlackboxOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            eventExtractor.Run(options, config);
        }

        private void moveScreenshots(string departure, string arrival)
        {
            var photo = new MoveScreenshotsCommand();
            var options = new MoveScreenshotsOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            photo.Run(options, config);
        }

        private void moveNavigraphCharts(string departure, string arrival)
        {
            var navi = new MoveNavigraphChartsCommand();
            var options = new MoveNavigraphChartsOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            navi.Run(options, config);
        }

        private void compressBlackbox(string departure, string arrival)
        {
            var compresser = new CompressBlackboxCommand();
            var options = new CompressBlackboxOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            compresser.Run(options, config);
        }

        private void geocodeScreenshots(string departure, string arrival)
        {
            var geocoder = new GeoTagScreenshotsCommand();
            var options = new GeoTagScreenshotsOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            geocoder.Run(options, config);
        }

        private void generateStats(string departure, string arrival)
        {
            var trimmer = new CombineStatsCommand();
            var options = new CombineStatsOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            trimmer.Run(options, config);
        }

        private void trimBlackbox(string departure, string arrival)
        {
            var trimmer = new TrimBlackboxCommand();
            var options = new TrimBlackboxOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            trimmer.Run(options, config);
        }

        private void generatePreviews(string departure, string arrival)
        {
            var previewer = new CreatePreviewForImagesCommand();
            var options = new CreatePreviewForImagesOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure,
                Height = 300
            };

            previewer.Run(options, config);
        }

        private void convertCharts(string departure, string arrival)
        {
            var converter = new ConvertChartsToImageCommand();
            var options = new ConvertChartsToImageOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            converter.Run(options, config);
        }

        private void renameImports(string departure, string arrival)
        {
            var renamer = new RenameImportFilesCommand();
            var options = new RenameImportFilesOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            renamer.Run(options, config);
        }

        private void processSimbrief(string departure, string arrival)
        {
            var simbrief = new ProcessSimbriefCommand();
            var options = new ProcessSimbriefOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            simbrief.Run(options, config);
        }

        private void recordBlackBox(string departure, string arrival)
        {
            var recorder = new BlackboxRecorderCommand();
            var options = new BlackboxRecorderOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            recorder.Run(options, config);
        }

        private void importFromSimToolkitPro(string departure, string arrival)
        {
            Console.WriteLine($"Press ENTER when you have completed the SimToolkitPro Flight.");
            Console.ReadLine();

            var toolkitSql = new ImportToolkitCommand();
            var options = new ImportToolkitOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            toolkitSql.Run(options, config);
        }

        //private void createAirportFiles(string departure, string arrival)
        //{
        //    var airports = new AirportCommand(config);
        //    var options = new AirportOptions()
        //    {
        //        ArrivalAirport = arrival,
        //        DepartureAirport = departure
        //    };

        //    airports.Run(options);
        //}

        //private void dispatchSimbrief(string departure, string arrival)
        //{
        //    var dispatcher = new DispatchCommand(config);
        //    var options = new DispatchOptions()
        //    {
        //        ArrivalAirport = arrival,
        //        DepartureAirport = departure
        //    };

        //    dispatcher.Run(options);
        //}

        //private void createFlightFolder(string departure, string arrival)
        //{
        //    var mkdir = new DirCommand(config);
        //    var options = new DirOptions()
        //    {
        //        ArrivalAirport = arrival,
        //        DepartureAirport = departure
        //    };

        //    mkdir.Run(options);
        //}
    }
}
