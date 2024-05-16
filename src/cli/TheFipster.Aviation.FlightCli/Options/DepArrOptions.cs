using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    public class DepArrOptions
    {
        public DepArrOptions()
        {
            
        }

        public DepArrOptions(string departure, string arrival)
        {
            DepartureAirport = departure; 
            ArrivalAirport = arrival;
        }

        [Option('d', "dep", Required = false, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = false, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
