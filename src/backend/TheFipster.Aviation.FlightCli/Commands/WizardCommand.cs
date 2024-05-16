using TheFipster.Aviation.Domain;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class WizardCommand
    {
        private HardcodedConfig config;

        public WizardCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(WizardOptions _)
        {
            Console.WriteLine("Hello Captain.");
            Console.WriteLine();

            var nextFlight = getNextFlight();
            var departure = nextFlight.Departure.Ident;
            var arrival = nextFlight.Arrival.Ident;

            if (string.IsNullOrWhiteSpace(departure) || string.IsNullOrWhiteSpace(arrival))
                throw new ApplicationException("Can't determine next flight.");

            createFlightFolder(departure, arrival);
            dispatchSimbrief(departure, arrival);
            createAirportFiles(departure, arrival);
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

        private PlannedFlight getNextFlight()
        {
            var next = new NextCommand(config);
            return next.Run();
        }

        private void combineGps(string departure, string arrival)
        {
            var gpsCombiner = new GpsCommand(config);
            var options = new GpsOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            gpsCombiner.Run(options);
        }

        private void extractEvents(string departure, string arrival)
        {
            var eventExtractor = new EventCommand(config);
            var options = new EventOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            eventExtractor.Run(options);
        }

        private void moveScreenshots(string departure, string arrival)
        {
            var photo = new PhotoCommand(config);
            var options = new PhotoOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            photo.Run(options);
        }

        private void moveNavigraphCharts(string departure, string arrival)
        {
            var navi = new NaviCommand(config);
            var options = new NaviOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            navi.Run(options);
        }

        private void dispatchSimbrief(string departure, string arrival)
        {
            var dispatcher = new DispatchCommand(config);
            var options = new DispatchOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            dispatcher.Run(options);
        }

        private void compressBlackbox(string departure, string arrival)
        {
            var compresser = new CompressCommand(config);
            var options = new CompressOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            compresser.Run(options);
        }

        private void geocodeScreenshots(string departure, string arrival)
        {
            var geocoder = new GeoTagCommand(config);
            var options = new GeoTagOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            geocoder.Run(options);
        }

        private void generateStats(string departure, string arrival)
        {
            var trimmer = new StatsCommand(config);
            var options = new StatsOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            trimmer.Run(options);
        }

        private void trimBlackbox(string departure, string arrival)
        {
            var trimmer = new TrimCommand(config);
            var options = new TrimOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            trimmer.Run(options);
        }

        private void generatePreviews(string departure, string arrival)
        {
            var previewer = new PreviewCommand(config);
            var options = new PreviewOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure,
                Height = 300
            };

            previewer.Run(options);
        }

        private void convertCharts(string departure, string arrival)
        {
            var converter = new ChartCommand(config);
            var options = new ChartOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            converter.Run(options);
        }

        private void renameImports(string departure, string arrival)
        {
            var renamer = new RenameCommand(config);
            var options = new RenameOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            renamer.Run(options);
        }

        private void extractFromSimbrief(string departure, string arrival)
        {
            var simbrief = new SimbriefCommand(config);
            var options = new SimbriefOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            simbrief.Run(options);
        }

        private void recordBlackBox(string departure, string arrival)
        {
            var recorder = new RecorderCommand(config);
            var options = new RecorderOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            recorder.Run(options);
        }

        private void extractFromSimToolkitPro(string departure, string arrival)
        {
            Console.WriteLine($"Press ENTER when you have completed the SimToolkitPro Flight.");
            Console.ReadLine();

            var toolkitSql = new ToolkitCommand(config);
            var options = new ToolkitOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            toolkitSql.Run(options);
        }

        private void createFlightFolder(string departure, string arrival)
        {
            var mkdir = new DirCommand(config);
            var options = new DirOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            mkdir.Run(options);
        }

        private void createAirportFiles(string departure, string arrival)
        {
            var airports = new AirportCommand(config);
            var options = new AirportOptions()
            {
                ArrivalAirport = arrival,
                DepartureAirport = departure
            };

            airports.Run(options);
        }
    }
}
