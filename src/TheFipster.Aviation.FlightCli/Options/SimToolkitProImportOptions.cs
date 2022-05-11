using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("stkp", HelpText = "Import data export file from SimToolkitPro.")]
    internal class SimToolkitProImportOptions
    {
        [Option('f', "file", Required = true, HelpText = "SimToolkitPro Exported File")]
        public string Filepath { get; set; }
    }
}
