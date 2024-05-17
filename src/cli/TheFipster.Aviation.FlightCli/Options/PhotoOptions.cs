using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("photo", HelpText = "Import the screenshots.")]
    internal class PhotoOptions : FlightRequiredOptions { }
}
