using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Merged;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.BlackBox;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class ScanCommand
    {
        private HardcodedConfig config;

        public ScanCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(ScanOptions _)
        {
            var flights = Scan();
            print(flights);
        }

        public Dictionary<string, Dictionary<string, FileTypes>> Scan()
        {
            var flights = new Dictionary<string, Dictionary<string, FileTypes>>();
            var flightFolders = Directory.GetDirectories(config.FlightsFolder).OrderBy(x => x);

            foreach (var flightFolder in flightFolders)
            {
                var flightName = Path.GetFileName(flightFolder);
                var flight = Scan(flightFolder);
                flights.Add(flightName, flight);
            }

            return flights;
        }

        private void print(Dictionary<string, Dictionary<string, FileTypes>> flights)
        {
            foreach (var flight in flights)
            {
                var folder = Path.GetFileName(flight.Key);
                Console.WriteLine(folder);
                Console.WriteLine();
                foreach (var file in flight.Value.OrderBy(x => Path.GetExtension(x.Key)).ThenBy(x => x.Value))
                {
                    switch (file.Value)
                    {
                        case FileTypes.Unknown:
                        case FileTypes.Error:
                        case FileTypes.Chart:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case FileTypes.SimbriefXml:
                        case FileTypes.OfpPdf:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            break;
                        case FileTypes.BlackBoxCsv:
                        case FileTypes.BlackBoxJson:
                        case FileTypes.TrackJson:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case FileTypes.SimbriefJson:
                        case FileTypes.SimToolkitProJson:
                        case FileTypes.AirportJson:
                        case FileTypes.LandingJson:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case FileTypes.ChartAirport:
                        case FileTypes.ChartApproach:
                        case FileTypes.ChartArrival:
                        case FileTypes.ChartDeparture:
                        case FileTypes.ChartParking:
                        case FileTypes.ChartTaxi:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case FileTypes.RouteKml:
                        case FileTypes.PathKml:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        case FileTypes.MsfsFlightPlan:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case FileTypes.MergedFlightJson:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                    Console.WriteLine($"\t{file.Value} - {file.Key}");
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public Dictionary<string, FileTypes> Scan(string flightFolder)
        {
            var flight = new Dictionary<string, FileTypes>();
            var files = Directory.GetFiles(flightFolder);
            foreach (var file in files)
            {
                var filetype = scanFile(file);
                flight.Add(file, filetype);
            }

            return flight;
        }

        private FileTypes scanFile(string file)
        {
            switch (Path.GetExtension(file))
            {
                case ".xml":
                    {
                        return scanXml(file);
                    }
                case ".csv":
                    {
                        return scanCsv(file);
                    }
                case ".pdf":
                    {
                        return scanPdf(file);
                    }
                case ".pln":
                    {
                        return FileTypes.MsfsFlightPlan;
                    }
                case ".kml":
                    {
                        return scanKml(file);
                    }
                case ".json":
                    {
                        return scanJson(file);
                    }
            }

            return FileTypes.Unknown;
        }

        private FileTypes scanJson(string file)
        {
            try
            {
                var data = new JsonReader<Track>().FromFile(file);
                if (data.Features != null)
                    return FileTypes.TrackJson;
            }
            catch (Exception _)
            {
            }

            try
            {
                var data = new JsonReader<SimBriefFlight>().FromFile(file);
                if (data.Departure != null)
                    return FileTypes.SimbriefJson;
            }
            catch (Exception _)
            {
            }

            try
            {
                var data = new JsonReader<BlackBoxFlight>().FromFile(file);
                if (data.Records != null && data.Records.Count > 0)
                    if (data.Records.Count > 1000)
                        return FileTypes.BlackBoxJson;
                    else
                        return FileTypes.Error;
            }
            catch (Exception _)
            {
            }

            try
            {
                var data = new JsonReader<Flight>().FromFile(file);
                if (!string.IsNullOrWhiteSpace(data.Route))
                    return FileTypes.MergedFlightJson;
            }
            catch (Exception _)
            {
            }

            try
            {
                var data = new JsonReader<SimToolkitProFlight>().FromFile(file);
                if (data.Logbook != null)

                if (data.Logbook.TrackedGeoJson.Length > 1000)
                    return FileTypes.SimToolkitProJson;
                else
                    return FileTypes.Error;
            }
            catch (Exception _)
            {
            }

            try
            {
                var data = new JsonReader<LandingReport>().FromFile(file);
                if (data.Landing != null)
                    return FileTypes.LandingJson;
            }
            catch (Exception _)
            {
            }

            try
            {
                var data = new JsonReader<Domain.Datahub.Airport>().FromFile(file);
                if (!string.IsNullOrWhiteSpace(data.Ident))
                    return FileTypes.AirportJson;
            }
            catch (Exception _)
            {
            }

            return FileTypes.Unknown;
        }

        private FileTypes scanKml(string file)
        {
            if (File.ReadLines(file).Any(x => x == "<Point>"))
                return FileTypes.RouteKml;

            return FileTypes.PathKml;
        }

        private FileTypes scanXml(string file)
        {
            var secondLine = File.ReadLines(file).Skip(1).FirstOrDefault();
            if (secondLine == "<OFP>")
                return FileTypes.SimbriefXml;

            return FileTypes.Unknown;
        }

        private FileTypes scanCsv(string file)
        {
            var text = File.ReadLines(file).FirstOrDefault();

            if (text == CsvWriter.Header)
                return FileTypes.BlackBoxCsv;

            return FileTypes.Unknown;
        }

        private FileTypes scanPdf(string file)
        {
            var filename = Path.GetFileName(file);

            if (filename.Contains("OFP") || filename.Contains("_PDF_"))
                return FileTypes.OfpPdf;

            if (filename.Contains("Airport"))
                return FileTypes.ChartAirport;
            if (filename.Contains("Parking"))
                return FileTypes.ChartParking;
            if (filename.Contains("APP"))
                return FileTypes.ChartApproach;
            if (filename.Contains("STAR"))
                return FileTypes.ChartArrival;
            if (filename.Contains("SID"))
                return FileTypes.ChartDeparture;
            if (filename.Contains("Taxi"))
                return FileTypes.ChartTaxi;

            return FileTypes.Chart;
        }
    }
}
