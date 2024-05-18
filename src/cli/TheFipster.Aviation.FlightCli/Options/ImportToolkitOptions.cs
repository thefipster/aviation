using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("toolkit", HelpText = "Reads STKP database file and outputs logbook, track and landing.")]
    public class ImportToolkitOptions : FlightOptions { }
}
