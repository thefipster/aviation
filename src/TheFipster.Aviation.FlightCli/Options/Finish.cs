using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("fin", HelpText = "Finishes the paperwork for the flight.")]
    internal class FinishOptions
    {
        [Option(Required = true)]
        public string? Flight { get; set; }
    }
}
