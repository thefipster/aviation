using System.Security.Cryptography;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class ScanCommand
    {
        private HardcodedConfig config;
        private ScanOptions options;

        public ScanCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(ScanOptions options)
        {
            this.options = options;
            var flights = Scan();
            print(flights);
        }

        public Dictionary<string, Dictionary<string, FileTypes>> Scan()
        {
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];


            var flights = new Dictionary<string, Dictionary<string, FileTypes>>();
            foreach (var folder in folders)
            {
                var flightName = Path.GetFileName(folder);
                var flight = new FlightFileScanner().GetFiles(folder);
                flights.Add(flightName, flight);
            }

            return flights;
        }

        private void print(Dictionary<string, Dictionary<string, FileTypes>> flights)
        {
            var totalCount = 0;
            foreach (var flight in flights)
            {
                totalCount += flight.Value.Count();
                var folder = Path.GetFileName(flight.Key);
                Console.WriteLine($"{folder} - {flight.Value.Count()} files");
                Console.WriteLine();
                foreach (var file in flight.Value.OrderBy(x => x.Key))
                {
                    switch (file.Value)
                    {
                        case FileTypes.Unknown:
                        case FileTypes.Error:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;
                        case FileTypes.SimbriefXml:
                        case FileTypes.SimbriefJson:
                        case FileTypes.OfpPdf:
                        case FileTypes.RouteKml:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;
                        case FileTypes.NotamJson:
                        case FileTypes.WaypointsJson:
                        case FileTypes.RouteJson:
                        case FileTypes.OfpHtml:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case FileTypes.SimToolkitProJson:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            break;
                        case FileTypes.TrackJson:
                        case FileTypes.LandingJson:
                        case FileTypes.LogbookJson:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;
                        case FileTypes.Chart:
                        case FileTypes.ChartImage:
                        case FileTypes.ChartThumbnail:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            break;
                        case FileTypes.MsfsFlightPlan:
                        case FileTypes.PathKml:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        case FileTypes.Screenshot:
                        case FileTypes.Thumbnail:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case FileTypes.AirportJson:
                        case FileTypes.MergedFlightJson:
                        case FileTypes.BlackBoxCsv:
                        case FileTypes.BlackBoxJson:
                        case FileTypes.BlackBoxTrimmedJson:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                    Console.WriteLine($"\t{file.Value.ToString().PadRight(20)} - {file.Key}");
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine($"Total: {totalCount} files.");
            Console.WriteLine();
        }
    }
}
