using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("preview", HelpText = "Resizes the screenshots for preview.")]
    internal class PreviewOptions : DepArrOptions
    {
        [Option('h', "height", Required = true, HelpText = "Max height of preview")]
        public int Height { get; set; }
    }
}
