using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("rec", HelpText = "Record a flight in MSFS2020.")]
    internal class RecorderOptions : DepArrOptions { }
}
