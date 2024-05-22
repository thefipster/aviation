using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb(Verb, HelpText = Help)]
    public class ImportProcessorOptions : FlightOptions
    {
        public const string Verb = "process";
        public const string Help = "Processes the import file into a more managable format.";
        public const string Welcome = "Processing imports";
    }
}
