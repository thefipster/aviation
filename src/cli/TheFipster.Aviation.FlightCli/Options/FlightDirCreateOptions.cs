
using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("dir", HelpText = "Create the flight folder.")]
    public class FlightDirCreateOptions : FlightRequiredOptions { }
}
