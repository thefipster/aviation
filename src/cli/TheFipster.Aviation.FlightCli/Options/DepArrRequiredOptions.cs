using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    public class DepArrRequiredOptions
    {
        public DepArrRequiredOptions() { }

        public DepArrRequiredOptions(string departure, string arrival)
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
