using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("navi", HelpText = "Imports printed pdf charts from Navigraph.")]
    internal class NaviOptions : FlightRequiredOptions { }
}
