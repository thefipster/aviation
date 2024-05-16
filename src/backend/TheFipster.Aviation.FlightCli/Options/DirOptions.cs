
using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("dir", HelpText = "Create the flight folder.")]
    internal class DirOptions : DepArrRequiredOptions { }
}
