using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("toolkit", HelpText = "Reads STKP database file and outputs logbook, track and landing.")]
    internal class ToolkitOptions
    {
        [Option('d', "dep", Required = false, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = false, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
