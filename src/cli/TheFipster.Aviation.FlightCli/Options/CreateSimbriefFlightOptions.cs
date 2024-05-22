using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    public class CreateSimbriefFlightOptions : IOptions
    {
        public const string Verb = "simbriefflight";
        public const string Help = "Creating flight from the last dispatched Simbrief flight.";
        public const string Welcome = Help;
    }
}
