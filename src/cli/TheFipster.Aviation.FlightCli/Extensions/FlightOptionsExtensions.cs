using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Extensions
{
    public static class FlightOptionsExtensions
    {
        public static IEnumerable<string> GetFlightFolders(this FlightOptions options, string flightsFolder)
            => hasSufficientOptions(options)
                ? getSpecifiedFlightFolder(options, flightsFolder)
                : getAllFlightFolders(flightsFolder);

        private static bool hasSufficientOptions(FlightOptions options)
            => !string.IsNullOrWhiteSpace(options.DepartureAirport) && !string.IsNullOrWhiteSpace(options.ArrivalAirport);

        private static IEnumerable<string> getSpecifiedFlightFolder(FlightOptions options, string flightsFolder)
            => [new FlightFinder().GetFlightFolder(flightsFolder, options.DepartureAirport, options.ArrivalAirport)];

        private static IEnumerable<string> getAllFlightFolders(string flightsFolder)
            => new FlightFinder().GetFlightFolders(flightsFolder);
    }
}
