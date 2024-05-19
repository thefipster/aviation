using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb(Verb, HelpText = Help)]
    public class CombineImportsOptions : FlightOptions
    {
        public const string Verb = "import";
        public const string Help = "Combines all ascii input files into a single one for easier data access.";
        public const string Welcome = Help;
    }
}
