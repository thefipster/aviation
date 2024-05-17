using CommandLine;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("preview", HelpText = "Resizes the screenshots for preview.")]
    internal class PreviewOptions : FlightOptions
    {
        [Option('h', "height", Required = true, HelpText = "Max height of preview")]
        public int Height { get; set; }

        [Option('w', "width", Required = true, HelpText = "Max width of preview")]
        public int Width { get; set; }
    }
}
