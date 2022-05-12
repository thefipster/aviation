using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("merge", HelpText = "Merge all the imports into single data file.")]
    internal class MergeOptions
    {
        [Option('a', "airport", Required = true, Min = 2, Max = 3, HelpText = "Airport files, up to 3 files for departure, arrival and alternate.")]
        public IEnumerable<string> AirportFiles { get; set; }

        [Option('b', "blackbox", Required = true, HelpText = "BlackBox recording json file.")]
        public string BlackboxFile { get; set; }

        [Option('s', "simbrief", Required = true, HelpText = "Simbrief import json file.")]
        public string SimbriefFile { get; set; }

        [Option('t', "toolkit", Required = true, HelpText = "SimToolkitPro import json file.")]
        public string ToolkitFile { get; set; }
    }
}
