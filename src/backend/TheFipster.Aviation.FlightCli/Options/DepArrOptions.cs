using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    internal class DepArrOptions
    {
        [Option('d', "dep", Required = false, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = false, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
