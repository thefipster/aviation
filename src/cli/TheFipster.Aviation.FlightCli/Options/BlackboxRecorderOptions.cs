using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("record", HelpText = "Record a flight in Microsoft Flight Simulator 2020 via FSUIPC.")]
    internal class BlackboxRecorderOptions : FlightRequiredOptions { }
}
