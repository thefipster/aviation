using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb(Verb, HelpText = Help)]
    public class ImportCollectorOptions : FlightRequiredOptions
    {
        public const string Verb = "collect";
        public const string Help = "Collects the externally generated files for the flight.";
        public const string Welcome = "Collecting files from external tools.";
    }
}
