using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("airports", HelpText = "Creates airport files of departure, arrival and alternate of the flight or all if omitted.")]
    public class AirportFileGeneratorOptions : DepArrOptions { }
}
