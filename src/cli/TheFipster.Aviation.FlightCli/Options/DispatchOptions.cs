using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("dispatch", HelpText = "Import a dispatched Simbrief flight.")]
    public class DispatchOptions : DepArrRequiredOptions { }
}
