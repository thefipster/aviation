using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Extensions
{
    public static class FlightOptionsExtensions
    {
        public static IEnumerable<string> GetFlightFolders(this FlightOptions options, string flightsFolder)
            => hasSufficientOptions(options)
                ? getSpecifiedFlightFolder(options.DepartureAirport, options.ArrivalAirport, flightsFolder)
                : getAllFlightFolders(flightsFolder);
        public static string GetFlightFolder(this FlightRequiredOptions options, string flightsFolder)
            => getSpecifiedFlightFolder(options.DepartureAirport, options.ArrivalAirport, flightsFolder).First();

        private static bool hasSufficientOptions(FlightOptions options)
            => !string.IsNullOrWhiteSpace(options.DepartureAirport) && !string.IsNullOrWhiteSpace(options.ArrivalAirport);

        private static IEnumerable<string> getSpecifiedFlightFolder(string departure, string arrival, string flightsFolder)
            => [new FlightFinder().GetFlightFolder(flightsFolder, departure, arrival)];

        private static IEnumerable<string> getAllFlightFolders(string flightsFolder)
            => new FlightFinder().GetFlightFolders(flightsFolder);
    }
}
