using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("rec", HelpText = "Record a flight in MSFS2020.")]
    internal class RecorderOptions
    {
        [Option('d', "dep", Required = true, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = true, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
