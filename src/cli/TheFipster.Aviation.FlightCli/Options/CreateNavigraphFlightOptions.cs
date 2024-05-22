using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    public class CreateNavigraphFlightOptions : IOptions
    {
        public const string Verb = "navigraphflight";
        public const string Help = "Creating flight from navigraph export.";
        public const string Welcome = Help;
    }
}
