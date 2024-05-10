using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.BlackBox;
using TheFipster.Aviation.Modules.Simbrief;
using TheFipster.Aviation.Modules.Simbrief.Components;
using TheFipster.Aviation.Modules.SimToolkitPro;

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
            var departure = getAirport("departure");
            var arrival = getAirport("arrival");

            dispatchSimbrief(departure.Ident, arrival.Ident);
            var flightPath = createFlightFolder(departure.Ident, arrival.Ident);
            var simbriefFlight = moveSimbriefFiles(flightPath, departure.Ident, arrival.Ident);

            createAirportFiles(simbriefFlight, flightPath);
            recordBlackBox(departure, arrival, flightPath);
            moveNavigraphCharts(flightPath);
            moveScreenshots(flightPath);

            extractFromSimToolkitPro(departure, arrival, flightPath);
            extractFromSimbrief(flightPath);

            renameImports(departure.Ident, arrival.Ident, flightPath);
        }

        private void renameImports(string departure, string arrival, string flightPath)
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

        private SimBriefFlight extractFromSimbrief(string flightPath)
        {
            var flight = new SimbriefImporter().Import(flightPath);
            return flight;
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

        private BlackBoxFlight recordBlackBox(Airport departure, Airport arrival, string flightPath)
        {
            var recorder = new RecorderCommand(config);
            var blackbox = recorder.Record(departure.Ident, arrival.Ident);
            new JsonWriter<BlackBoxFlight>().Write(flightPath, blackbox, FileTypes.BlackBoxJson, departure.Ident, arrival.Ident);
            new BlackBoxCsvWriter().Write(flightPath, blackbox, FileTypes.BlackBoxCsv, departure.Ident, arrival.Ident);
            Console.Clear();
            return blackbox;
        }

        private SimToolkitProFlight extractFromSimToolkitPro(Airport departure, Airport arrival, string flightPath)
        {
            Console.WriteLine($"Press ENTER when you have completed the SimToolkitPro Flight.");
            Console.ReadLine();

            var flight = new SimToolkitProImporter()
                .Import(
                    flightPath, 
                    config.SimToolkitProDatabaseFile, 
                    departure.Ident, 
                    arrival.Ident);

            return flight;
        }

        private Airport getAirport(string type)
        {
            Console.WriteLine($"Enter your {type} Airport (ICAO):");
            Airport? airport = null;
            do
            {
                var icaoCode = Console.ReadLine();
                if (!string.IsNullOrEmpty(icaoCode))
                    airport = new Modules.Airports.AirportFinder(config.AirportFile).SearchWithIcao(icaoCode.ToUpper());

                if (airport != null)
                    Console.WriteLine(airport.Name);
                else
                    Console.WriteLine("Try again mate...");
            }
            while (airport == null);
            return airport;
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

        private void createAirportFiles(SimBriefFlight simbriefData, string flightPath)
        {
            Console.WriteLine($"Creating airport files:");
            createAirportFile(simbriefData.Departure.Icao, flightPath, true);
            createAirportFile(simbriefData.Arrival.Icao, flightPath, true);
            createAirportFile(simbriefData.Alternate.Icao, flightPath, false);
        }

        private void createAirportFile(string icao, string flightPath, bool isRequired)
        {
            var airport = new Modules.Airports.AirportFinder(config.AirportFile).SearchWithIcao(icao);
            if (isRequired && airport == null)
                throw new AirportNotFoundException("Couldn't locate {icao} in airport data file.");

            if (airport != null)
            {
                new JsonWriter<Airport>().Write(flightPath, airport, FileTypes.AirportJson, airport.Ident);
                Console.WriteLine($"\t {icao} - {airport.Name}");
            }
        }
    }
}
