using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("toolkit", HelpText = "Reads STKP database file and outputs logbook, track and landing.")]
    internal class ToolkitOptions : FlightOptions { }
}
