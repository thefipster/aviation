using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("chart", HelpText = "Converts pdf charts into image files.")]
    internal class ChartOptions : FlightOptions { }
}
