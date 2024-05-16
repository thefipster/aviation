using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("event", HelpText = "Scans the blackbox for configuration events and record values.")]
    internal class EventOptions : DepArrOptions { }
}
