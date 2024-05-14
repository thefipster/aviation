using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("optimize", HelpText = "Optimizes STKP track file.")]
    internal class OptimizeOptions : DepArrOptions { }
}
