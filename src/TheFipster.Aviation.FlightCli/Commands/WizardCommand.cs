using FSUIPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Airports;
using TheFipster.Aviation.Modules.BlackBox;
using TheFipster.Aviation.Modules.Simbrief.Components;
using TheFipster.Aviation.Modules.SimToolkitPro.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class WizardCommand
    {
        private HardcodedConfig config;

        public WizardCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(WizardOptions o)
        {
            var departure = getAirport("departure");
            var arrival = getAirport("arrival");
            var simbriefFiles = dispatchSimbrief(departure, arrival);
            var flightPath = createFlightFolder(departure, arrival);
            var simbriefData = moveSimbriefFiles(simbriefFiles, flightPath);
            createAirportFiles(simbriefData, flightPath);
            //var blackbox = recordBlackBox(departure, arrival, flightPath);
            //var stkpFlight = extractStkpFlight(departure, arrival, flightPath);

            moveNavigraphCharts(flightPath);
        }

        private void moveNavigraphCharts(string flightPath)
        {
            Console.WriteLine($"Print the used charts from Navigraph as pdf into the folder {config.NavigraphFolder}");
            Console.WriteLine("When you're ready press ENTER.");
            Console.ReadLine();
            Console.WriteLine($"Moving charts to {flightPath}");
            var navigraphFiles = Directory.GetFiles(config.NavigraphFolder);
            foreach (var file in navigraphFiles)
            {
                var filename = Path.GetFileName(file);
                var newFile = Path.Combine(flightPath, filename);
                File.Move(file, newFile);
                Console.WriteLine($"\t {filename}");
            }
        }

        private BlackBoxFlight recordBlackBox(Airport departure, Airport arrival, string flightPath)
        {
            var recorder = new RecorderCommand();
            var blackbox = recorder.Record(departure.Ident, arrival.Ident);
            new JsonWriter<BlackBoxFlight>().Write(flightPath, blackbox, "BlackBox", departure.Ident, arrival.Ident);
            new CsvWriter().Write(flightPath, blackbox);
            Console.Clear();
            return blackbox;
        }

        private SimToolkitProFlight extractStkpFlight(Airport departure, Airport arrival, string flightPath)
        {
            Console.WriteLine($"Please create a SimToolkitPro data export into the folder {config.SimToolkitProFolder} and tell me when the download finished by pressing ENTER.");
            Console.ReadLine();
            var filepath = new Modules.SimToolkitPro.Components.Finder().Find(config.SimToolkitProFolder);
            var flightsList = new Modules.SimToolkitPro.Components.Loader().Read(filepath);
            var flight = flightsList.FirstOrDefault(x => x.Logbook.Dep == departure.Ident && x.Logbook.Arr == arrival.Ident);
            if (flight == null)
                throw new ApplicationException($"Couldn't find flight in SimToolkitPro Export {filepath}.");

            new JsonWriter<SimToolkitProFlight>().Write(flightPath, flight, "SimToolkitPro", departure.Ident, arrival.Ident);
            Console.WriteLine($"Extracting STKP flight information from {filepath}.");

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
                    airport = new Modules.Airports.Finder(config.AirportFile).SearchWithIcao(icaoCode.ToUpper());

                if (airport != null)
                    Console.WriteLine(airport.Name);
                else
                    Console.WriteLine("Try again mate...");
            }
            while (airport == null);
            return airport;
        }

        private IEnumerable<string> dispatchSimbrief(Airport departure, Airport arrival)
        {
            Console.WriteLine("Dispatch your flight with SimBrief. When the files are synced this continues...");
            List<string> simbriefFiles = new List<string>();
            do
            {
                Thread.Sleep(1000);
                simbriefFiles = new Modules.Simbrief.Components.Finder().FindExportFiles(config.SimbriefFolder, departure.Ident, arrival.Ident).ToList();

                if (simbriefFiles.Count > 0)
                {
                    Console.WriteLine("Simbrief export detected. Waiting until download finished.");
                    Thread.Sleep(5000);
                    simbriefFiles = new Modules.Simbrief.Components.Finder().FindExportFiles(config.SimbriefFolder, departure.Ident, arrival.Ident).ToList();
                }
            }
            while (simbriefFiles.Count == 0);
            return simbriefFiles;
        }

        private string createFlightFolder(Airport departure, Airport arrival)
        {
            var flightNo = Directory.GetDirectories(config.FlightsFolder).Count() + 1;
            var flightTerminators = $"{departure.Ident} - {arrival.Ident}";
            var flightName = $"{flightNo:D4} - {flightTerminators}";
            var flightPath = Path.Combine(config.FlightsFolder, flightName);

            if (Directory.GetDirectories(config.FlightsFolder, $"*{flightTerminators}").Any())
                throw new ApplicationException($"The flight from {departure.Name} to {arrival.Name} already exists.");

            Console.WriteLine($"Creating flight folder at {flightPath}.");
            Directory.CreateDirectory(flightPath);
            return flightPath;
        }

        private SimBriefFlight moveSimbriefFiles(IEnumerable<string> simbriefFiles, string flightPath)
        {
            Console.WriteLine($"Copying simbrief files to {flightPath}:");
            string? xmlFile = null;
            foreach (var oldFilepath in simbriefFiles)
            {
                var filename = Path.GetFileName(oldFilepath);
                var newFilepath = Path.Combine(flightPath, filename);
                Console.WriteLine($"\t {filename}");
                File.Move(oldFilepath, newFilepath);

                if (Path.GetExtension(newFilepath) == ".xml")
                    xmlFile = newFilepath;
            }

            if (string.IsNullOrWhiteSpace(xmlFile) || !File.Exists(xmlFile))
                throw new ApplicationException("Simbrief XML data file couldn't be located.");

            Console.WriteLine($"Found data file: {xmlFile}.");
            var simbriefData = new Modules.Simbrief.Components.Loader().Read(xmlFile);

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
            var airport = new Modules.Airports.Finder(config.AirportFile).SearchWithIcao(icao);
            if (isRequired && airport == null)
                throw new ApplicationException("Couldn't locate {icao} in airport data file.");

            if (airport != null)
            {
                new JsonWriter<Airport>().Write(flightPath, airport, "Airport", airport.Ident);
                Console.WriteLine($"\t {icao} - {airport.Name}");
            }
        }
    }
}
