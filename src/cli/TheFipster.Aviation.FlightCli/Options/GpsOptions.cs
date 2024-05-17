using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("gps", HelpText = "Combines blackbox or track (blackbox wins) and waypoints into a single gps file.")]
    public class GpsOptions : FlightOptions
    {
    }
}
