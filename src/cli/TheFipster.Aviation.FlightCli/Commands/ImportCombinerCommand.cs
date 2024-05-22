using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Domain.Simbrief.Kml;
using TheFipster.Aviation.Domain.Simbrief.Xml;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class ImportCombinerCommand : IFlightCommand<ImportCombinerOptions>
    {
        private readonly FlightMeta meta;
        private readonly FlightFileScanner scanner;
        private readonly JsonReader<SimToolkitProFlight> stkpReader;
        private readonly JsonReader<BlackBoxFlight> blackboxReader;
        private readonly JsonReader<FlightImport> flightReader;
        private readonly JsonWriter<FlightImport> flightWriter;
        private readonly XmlReader xmlReader;
        private readonly JsonReader<SimbriefImport> simbriefImportReader;
        private readonly JsonReader<SimbriefXmlRaw> simbriefXmlRawReader;
        private readonly JsonReader<SimbriefKmlRaw> simbriefKmlRawReader;

        public ImportCombinerCommand()
        {
            meta = new FlightMeta();
            scanner = new FlightFileScanner();
            stkpReader = new JsonReader<SimToolkitProFlight>();
            blackboxReader = new JsonReader<BlackBoxFlight>();
            flightReader = new JsonReader<FlightImport>();
            flightWriter = new JsonWriter<FlightImport>();
            xmlReader = new XmlReader();
            simbriefImportReader = new JsonReader<SimbriefImport>();
            simbriefXmlRawReader = new JsonReader<SimbriefXmlRaw>();
            simbriefKmlRawReader = new JsonReader<SimbriefKmlRaw>();
        }

        public void Run(ImportCombinerOptions options, IConfig config)
        {
            Console.WriteLine(ImportCombinerOptions.Welcome);
            var folders = options.GetFlightFolders(config.FlightsFolder);
            foreach (var folder in folders)
            {
                Console.WriteLine("\t" + folder);
                var departure = meta.GetDeparture(folder);
                var arrival = meta.GetArrival(folder);
                var leg = meta.GetLeg(folder).ToString();
                FlightImport flight;

                try
                {
                    // Flight was already imported
                    var flightFile = scanner.GetFile(folder, FileTypes.FlightJson);
                    flight = flightReader.FromFile(flightFile);
                }
                catch (FileNotFoundException)
                {
                    // Nothing exists, let's start fresh
                    flight = new FlightImport(leg, departure, arrival);
                }

                LoadCreatedTime(folder, flight);
                LoadSimbriefXml(folder, flight);
                LoadSimbriefKml(folder, flight);
                LoadSimbriefJson(folder, flight);
                LoadSimToolkitPro(folder, flight);
                LoadBlackbox(folder, flight);

                flightWriter.Write(flight, folder, true);
            }
        }

        public FlightImport LoadCreatedTime(string folder, FlightImport flight)
        {
            try
            {
                var createdFile = scanner.GetFile(folder, FileTypes.CreatedTxt);
                var createdValue = File.ReadAllLines(createdFile).LastOrDefault();

                if (DateTime.TryParse(createdValue, out var startedDate))
                    flight.Started = startedDate;

                Console.WriteLine($"\t\t {Emoji.GreenCircle} Flight initiation date set.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"\t\t {Emoji.GreenCircle} Flight initiation date not available.");
            }

            return flight;
        }

        public FlightImport LoadSimbriefJson(string folder, FlightImport flight)
        {
            try
            {
                var simbriefFile = scanner.GetFile(folder, FileTypes.SimbriefImportJson);
                var simbrief = simbriefImportReader.FromFile(simbriefFile);

                flight.Simbrief = simbrief;
                Console.WriteLine($"\t\t {Emoji.GreenCircle} Simbrief JSON imported.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"\t\t {Emoji.YellowCircle} Simbrief JSON is not available.");
            }

            return flight;
        }

        public FlightImport LoadBlackbox(string folder, FlightImport flight)
        {
            try
            {
                var blackboxFile = scanner.GetFile(folder, FileTypes.BlackBoxJson);
                var blackbox = blackboxReader.FromFile(blackboxFile);

                flight.Blackbox = blackbox.Records;
                Console.WriteLine($"\t\t {Emoji.GreenCircle} Blackbox recording imported.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"\t\t {Emoji.YellowCircle} Blackbox recording is not available.");
            }

            return flight;
        }

        public FlightImport LoadSimToolkitPro(string folder, FlightImport flight)
        {
            try
            {
                var stkpFile = scanner.GetFile(folder, FileTypes.SimToolkitProJson);
                var stkpFlight = stkpReader.FromFile(stkpFile);

                flight.SimToolkitPro = stkpFlight;
                Console.WriteLine($"\t\t {Emoji.GreenCircle} SimToolkitPro export imported.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"\t\t {Emoji.YellowCircle} SimToolkitPro export is not available.");
            }

            return flight;
        }

        public FlightImport LoadSimbriefXml(string folder, FlightImport flight)
        {
            try
            {
                var simbriefFile = scanner.GetFile(folder, FileTypes.SimbriefXml);
                var simbriefJson = xmlReader.ReadToJson(simbriefFile);
                var xml = simbriefXmlRawReader.FromText(simbriefJson);
                flight.SimbriefXml = xml;

                Console.WriteLine($"\t\t {Emoji.GreenCircle} Simbrief XML imported.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"\t\t {Emoji.YellowCircle} Simbrief XML is not available.");
            }

            return flight;
        }

        public FlightImport LoadSimbriefKml(string folder, FlightImport flight)
        {
            try
            {
                var routeFile = scanner.GetFile(folder, FileTypes.RouteKml);
                var routeJson = xmlReader.ReadToJson(routeFile);
                var kml = simbriefKmlRawReader.FromText(routeJson);
                flight.SimbriefKml = kml;

                Console.WriteLine($"\t\t {Emoji.GreenCircle} Simbrief KML imported.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"\t\t {Emoji.YellowCircle} Simbrief KML is not available.");
            }

            return flight;
        }
    }
}
