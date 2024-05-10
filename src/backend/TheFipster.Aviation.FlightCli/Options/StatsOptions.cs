using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("stats", HelpText = "Scans flight and outputing some stats.")]
    internal class StatsOptions
    {
        [Option('d', "dep", Required = false, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = false, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
