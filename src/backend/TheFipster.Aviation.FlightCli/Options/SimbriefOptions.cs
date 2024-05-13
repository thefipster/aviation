using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("simbrief", HelpText = "Converts simbrief xml files into flight, notams and ofp.")]
    internal class SimbriefOptions
    {
        [Option('d', "dep", Required = false, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = false, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
