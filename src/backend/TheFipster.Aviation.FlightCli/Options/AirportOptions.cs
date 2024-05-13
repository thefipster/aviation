using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("airports", HelpText = "Creates airport files of departure, arrival and alternate of the flight or all if omitted.")]
    internal class AirportOptions
    {
        [Option('d', "dep", Required = false, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = false, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
