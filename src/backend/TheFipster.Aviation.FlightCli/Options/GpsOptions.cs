using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("gps", HelpText = "Combines blackbox or track (blackbox wins) and waypoints into a single gps file.")]
    internal class GpsOptions : DepArrOptions { }
}
