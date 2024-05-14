using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    internal class DepArrRequiredOptions
    {
        [Option('d', "dep", Required = true, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = true, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
