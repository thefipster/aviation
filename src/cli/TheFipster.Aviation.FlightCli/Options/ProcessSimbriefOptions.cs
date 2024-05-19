using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("simbrief", HelpText = "Converts simbrief xml files into flight, notams and ofp.")]
    public class ProcessSimbriefOptions : FlightOptions { }
}
