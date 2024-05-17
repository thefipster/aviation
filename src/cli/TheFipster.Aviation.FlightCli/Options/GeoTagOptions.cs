using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("geotag", HelpText = "Geo tags the screenshots.")]
    public class GeoTagOptions : FlightOptions { }
}
