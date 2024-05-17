using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("next", HelpText = "GEt departure and arrival of the next leg.")]
    internal class NextOptions : IOptions
    {

    }
}
