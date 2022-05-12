using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("simbrief", HelpText = "Import xml export file from Simbrief dispatch.")]
    internal class SimbriefImportOptions
    {
        [Option('f', "file", Required = true, HelpText = "Simbrief XML File")]
        public string Filepath { get; set; }
    }
}
