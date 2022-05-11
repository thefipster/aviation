using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("airpt", HelpText = "Writes a json file with infos about the airport.")]
    internal class AirportDetailsOptions
    {
        [Option('i', "icao", Required = true, HelpText = "Airport ICAO Code. Cologne would be EDDK")]
        public string AirportIcaoCode { get; set; }
    }
}
