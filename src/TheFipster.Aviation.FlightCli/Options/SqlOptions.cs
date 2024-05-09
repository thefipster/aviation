using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("sql", HelpText = "Reads the SQLite db from SimToolitPro to get flight and landing information.")]
    internal class SqlOptions
    {
        [Option('d', "dep", Required = true, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = true, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
