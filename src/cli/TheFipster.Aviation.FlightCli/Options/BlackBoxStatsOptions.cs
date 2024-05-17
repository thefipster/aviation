using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("event", HelpText = "Scans the blackbox for configuration events and record values.")]
    public class BlackBoxStatsOptions : FlightOptions { }
}
