using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class WizardCommand : ICommand<WizardOptions>
    {
        private IConfig config;

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

            runRequiredFlightCommand<FlightDirCreateCommand, FlightDirCreateOptions>(departure, arrival);
            //createFlightFolder(departure, arrival);

            runRequiredFlightCommand<SimbriefDispatchMoveCommand, SimbriefDispatchMoveOptions>(departure, arrival);
            //dispatchSimbrief(departure, arrival);

            runGenericFlightCommand<AirportFileGeneratorCommand, AirportFileGeneratorOptions>(departure, arrival);
            //createAirportFiles(departure, arrival);

            recordBlackBox(departure, arrival);
            moveNavigraphCharts(departure, arrival);
            moveScreenshots(departure, arrival);
            geocodeScreenshots(departure, arrival);
            extractFromSimToolkitPro(departure, arrival);
            extractFromSimbrief(departure, arrival);
            renameImports(departure, arrival);
            convertCharts(departure, arrival);
            generatePreviews(departure, arrival);
            trimBlackbox(departure, arrival);
            compressBlackbox(departure, arrival);
            extractEvents(departure, arrival);
            combineGps(departure, arrival);
            generateStats(departure, arrival);
        }

        private void runGenericFlightCommand<Tc, To>(string departure, string arrival)
            where Tc : IFlightCommand<To>, new()
            where To : FlightOptions, new()
        {
            var command = new Tc();
            var options = new FlightOptions(departure, arrival);
            command.Run((To)options, new HardcodedConfig());
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
            var gpsCombiner = new GpsCommand();
            var options = new GpsOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            gpsCombiner.Run(options, config);
        }

        private void extractEvents(string departure, string arrival)
        {
            var eventExtractor = new BlackBoxStatsCommand();
            var options = new BlackBoxStatsOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            eventExtractor.Run(options, config);
        }

        private void moveScreenshots(string departure, string arrival)
        {
            var photo = new PhotoCommand();
            var options = new PhotoOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            photo.Run(options, config);
        }

        private void moveNavigraphCharts(string departure, string arrival)
        {
            var navi = new NaviCommand();
            var options = new NaviOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            navi.Run(options, config);
        }

        private void compressBlackbox(string departure, string arrival)
        {
            var compresser = new CompressCommand();
            var options = new CompressOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            compresser.Run(options, config);
        }

        private void geocodeScreenshots(string departure, string arrival)
        {
            var geocoder = new GeoTagCommand();
            var options = new GeoTagOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            geocoder.Run(options, config);
        }

        private void generateStats(string departure, string arrival)
        {
            var trimmer = new StatsCommand();
            var options = new StatsOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            trimmer.Run(options, config);
        }

        private void trimBlackbox(string departure, string arrival)
        {
            var trimmer = new TrimCommand();
            var options = new TrimOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            trimmer.Run(options, config);
        }

        private void generatePreviews(string departure, string arrival)
        {
            var previewer = new PreviewCommand();
            var options = new PreviewOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure,
                Height = 300
            };

            previewer.Run(options, config);
        }

        private void convertCharts(string departure, string arrival)
        {
            var converter = new ChartCommand();
            var options = new ChartOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            converter.Run(options, config);
        }

        private void renameImports(string departure, string arrival)
        {
            var renamer = new RenameCommand();
            var options = new RenameOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            renamer.Run(options, config);
        }

        private void extractFromSimbrief(string departure, string arrival)
        {
            var simbrief = new SimbriefCommand();
            var options = new SimbriefOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            simbrief.Run(options, config);
        }

        private void recordBlackBox(string departure, string arrival)
        {
            var recorder = new RecorderCommand();
            var options = new RecorderOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            recorder.Run(options, config);
        }

        private void extractFromSimToolkitPro(string departure, string arrival)
        {
            Console.WriteLine($"Press ENTER when you have completed the SimToolkitPro Flight.");
            Console.ReadLine();

            var toolkitSql = new ToolkitCommand();
            var options = new ToolkitOptions()
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
