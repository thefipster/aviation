using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    public class FlightOptions : IOptions
    {
        public FlightOptions()
        {
            
        }

        public FlightOptions(string departure, string arrival)
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
