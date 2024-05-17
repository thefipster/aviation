using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("wizard", HelpText = "This wizard will guide you through the setup of your world tour flight.")]
    public class WizardOptions : IOptions
    {
    }
}
