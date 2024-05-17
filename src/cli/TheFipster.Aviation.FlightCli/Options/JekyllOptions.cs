using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("jekyll", HelpText = "Generates output for jekyll static site generator.")]
    public class JekyllOptions : FlightOptions { }
}
