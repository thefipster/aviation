using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb(Verb, HelpText = Help, Hidden = true)]
    public class CreateNavigraphFlightOptions : IOptions
    {
        public const string Verb = "navigraph";
        public const string Help = "Create a new flight with a Navigraph KML export.";
        public const string Welcome = "Creating new flight with Navigraph KML export.";
    }
}
