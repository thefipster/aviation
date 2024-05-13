using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief.Components;

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

            dispatchSimbrief(departure, arrival);
            var flightPath = createFlightFolder(departure, arrival);
            moveSimbriefFiles(flightPath, departure, arrival);

            createAirportFiles(departure, arrival);
            recordBlackBox(departure, arrival);

            moveNavigraphCharts(flightPath);
            moveScreenshots(flightPath);

            extractFromSimToolkitPro(departure, arrival);
            extractFromSimbrief(departure, arrival);
            renameImports(departure, arrival);
            convertCharts(departure, arrival);
            generatePreviews(departure, arrival);
            trimBlackbox(departure, arrival);
            generateStats(departure, arrival);
        }

        private PlannedFlight getNextFlight()
        {
            Console.WriteLine("You're next flight will be:");
            var next = new NextCommand(config);
            next.Run();
            return next.GetNext();
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

        private void moveScreenshots(string flightPath)
        {
            Console.WriteLine($"Moving screenshots from {config.ScreenshotFolder} --> {flightPath}");
            var files = new FileOperations().MoveFiles(config.ScreenshotFolder, flightPath, "Microsoft Flight Simulator*.png");
            foreach (var file in files)
                Console.WriteLine($"\t {Path.GetFileName(file)}");
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

        private void moveNavigraphCharts(string flightPath)
        {
            Console.WriteLine($"Print the used charts from Navigraph as pdf into the folder {config.NavigraphFolder}");
            Console.WriteLine("When you're ready press ENTER.");
            Console.ReadLine();
            Console.WriteLine($"Moving charts: {config.NavigraphFolder} --> {flightPath}");
            var files = new FileOperations().MoveFiles(config.NavigraphFolder, flightPath);

            foreach(var file in files )
                Console.WriteLine($"\t {Path.GetFileName(file)}");
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

        private IEnumerable<string> dispatchSimbrief(string departure, string arrival)
        {
            Console.WriteLine("Dispatch your flight with SimBrief. When the files are synced this continues...");
            List<string> simbriefFiles = new List<string>();
            do
            {
                Thread.Sleep(1000);
                simbriefFiles = new SimbriefFinder().FindExportFiles(config.SimbriefFolder, departure, arrival).ToList();

                if (simbriefFiles.Count > 0)
                {
                    Console.WriteLine("Simbrief export detected. Waiting until download finished.");
                    Thread.Sleep(5000);
                    simbriefFiles = new SimbriefFinder().FindExportFiles(config.SimbriefFolder, departure, arrival).ToList();
                }
            }
            while (simbriefFiles.Count == 0);
            return simbriefFiles;
        }

        private string createFlightFolder(string departure, string arrival)
        {
            var flightPath = new FileOperations()
                .CreateFlightFolder(
                config.FlightPlanFile, 
                config.FlightsFolder, 
                departure, 
                arrival);

            Console.WriteLine($"Creating flight folder at {flightPath}.");
            return flightPath;
        }

        private SimBriefFlight moveSimbriefFiles(string flightPath, string departure, string arrival)
        {
            Console.WriteLine($"Copying simbrief files to {flightPath}:");
            var searchPattern = $"{departure}{arrival}*";
            new FileOperations().MoveFiles(config.SimbriefFolder, flightPath, searchPattern);

            var files = new FlightFileScanner().GetFiles(flightPath, FileTypes.SimbriefXml);
            if (!files.Any())
                throw new FileNotFoundException("Couldn't find SimBrief XML file.");

            Console.WriteLine($"Found data file: {files.First()}.");
            var simbriefData = new SimbriefXmlLoader().Read(files.First());
            return simbriefData;
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
