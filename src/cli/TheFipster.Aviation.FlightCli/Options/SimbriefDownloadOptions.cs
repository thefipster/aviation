using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb(Verb, HelpText = Help)]
    public class SimbriefDownloadOptions : FlightRequiredOptions
    {
        public const string Verb = "download";
        public const string Help = "Downloads the maps from Simbrief.";
        public const string Welcome = Help;
    }
}
