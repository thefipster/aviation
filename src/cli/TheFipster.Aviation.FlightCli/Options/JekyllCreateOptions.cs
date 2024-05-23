using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("flog", HelpText = "Generated the output data for the flog.")]
    public class JekyllCreateOptions : FlightOptions { }
}
