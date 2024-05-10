using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("chart", HelpText = "Converts pdf charts into image files.")]
    internal class ChartOptions
    {
        [Option('d', "dep", Required = false, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = false, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
