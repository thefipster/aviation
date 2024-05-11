using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("trim", HelpText = "Trims the black box files to contain only engine on section.")]
    internal class TrimOptions
    {
        [Option('d', "dep", Required = false, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = false, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
