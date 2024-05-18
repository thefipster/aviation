using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("rec", HelpText = "Record a flight in MSFS2020.")]
    internal class BlackboxRecorderOptions : FlightRequiredOptions { }
}
