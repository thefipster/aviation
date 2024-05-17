using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    public class FlightRequiredOptions : IOptions
    {
        public FlightRequiredOptions() { }

        public FlightRequiredOptions(string departure, string arrival)
        {
            DepartureAirport = departure;
            ArrivalAirport = arrival;
        }

        [Option('d', "dep", Required = true, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = true, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
