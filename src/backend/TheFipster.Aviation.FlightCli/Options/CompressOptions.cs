using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("compress", HelpText = "Compresses the blackbox file.")]
    internal class CompressOptions : DepArrOptions { }
}
