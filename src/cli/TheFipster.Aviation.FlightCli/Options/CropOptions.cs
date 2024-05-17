using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("crop", HelpText = "Crops the title bar on the screenshots.")]
    public class CropOptions : FlightOptions { }
}
