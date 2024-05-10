using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("rename", HelpText = "Renames imported files into proper format. Only works after post processing is done.")]
    internal class RenameOptions
    {
        [Option('d', "dep", Required = false, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = false, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }
    }
}
