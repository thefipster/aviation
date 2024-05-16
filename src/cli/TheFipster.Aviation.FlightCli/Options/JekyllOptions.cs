using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("jekyll", HelpText = "Generates output for jekyll static site generator.")]
    internal class JekyllOptions : DepArrOptions { }
}
