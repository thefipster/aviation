using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("dispatch", HelpText = "Import a dispatched Simbrief flight.")]
    public class SimbriefDispatchMoveOptions : FlightRequiredOptions { }
}
