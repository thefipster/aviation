using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb(Verb, HelpText = Help, Hidden = true)]
    public class CreateSimbriefFlightOptions : IOptions
    {
        public const string Verb = "simbrief";
        public const string Help = "Create a flight from the latest dispatched Simbrief flight.";
        public const string Welcome = "Creating a new flight with the latest Simbrief dispatch.";
    }
}
