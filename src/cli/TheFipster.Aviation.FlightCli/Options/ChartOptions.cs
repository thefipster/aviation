using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("chart", HelpText = "Converts pdf charts into image files.")]
    public class ChartOptions : FlightOptions { }
}
