using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("scan", HelpText = "Scans all flight and outputs the available data sources for all of them.")]
    internal class ScanOptions : DepArrOptions
    {
    }
}
