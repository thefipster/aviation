using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("ourairports", HelpText = "Reads in the OurAirport csv files and outputs a filtered combined json file.")]
    public class OurAirportsFilterOptions : IOptions
    {
        [Option('i', "import", Required = true, HelpText = "Folder containing the OurAirport csv files.")]
        public string? ImportFolder { get; set; }

        [Option('o', "output", Required = true, HelpText = "Folder where the resulting json files will be written to.")]
        public string? OutputFolder { get; set; }
    }
}
