using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("geotag", HelpText = "Geo tags the screenshots.")]
    internal class GeoTagOptions : FlightOptions { }
}
