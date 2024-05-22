using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb(Verb, HelpText = Help)]
    public class NewDispatchFlightOptions : IOptions
    {
        public const string Verb = "newdispatch";
        public const string Help = "Start a flight dispatched by simbrief.";
        public const string Welcome = "Simbrief dispatcher mode";
    }
}
