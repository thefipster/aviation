using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("stats", HelpText = "Scans flight and outputing some stats.")]
    internal class StatsOptions : FlightOptions { }
}
