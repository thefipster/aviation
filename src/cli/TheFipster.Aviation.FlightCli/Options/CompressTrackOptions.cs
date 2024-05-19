using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("optimize", HelpText = "Optimizes STKP track file.")]
    internal class CompressTrackOptions : FlightOptions
    {
    }
}
