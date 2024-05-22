using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb(Verb, HelpText = Help)]
    public class NewManualFlightOptions : IOptions
    {
        public const string Verb = "newflight";
        public const string Help = "Start a new manual planned flight.";
        public const string Welcome = "Manual flight planning mode";
    }
}
