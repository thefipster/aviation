using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("preview", HelpText = "Resizes the screenshots for preview.")]
    internal class PreviewOptions
    {
        [Option('d', "dep", Required = false, HelpText = "Departure Airport ICAO")]
        public string? DepartureAirport { get; set; }

        [Option('a', "arr", Required = false, HelpText = "Arrival Airport ICAO")]
        public string? ArrivalAirport { get; set; }

        [Option('h', "height", Required = true, HelpText = "Max height of preview")]
        public int Height { get; set; }
    }
}
