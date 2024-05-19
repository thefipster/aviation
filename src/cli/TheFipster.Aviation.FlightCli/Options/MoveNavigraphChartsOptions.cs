using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("navi", HelpText = "Imports printed pdf charts from Navigraph.")]
    public class MoveNavigraphChartsOptions : FlightRequiredOptions { }
}
