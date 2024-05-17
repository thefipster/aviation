using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb(Verb, HelpText = Help)]
    public class AirportFileGeneratorOptions : FlightOptions 
    {
        public const string Verb = "airports";
        public const string Help = "Creates airport files of departure, arrival and alternate of the flight or all if omitted.";
        public const string Welcome = "Creating airport files for departure, arrival and alternate.";
    }
}
