using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("compress", HelpText = "Compresses the blackbox file.")]
    public class CompressOptions : FlightOptions
    {
    }
}
