using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb(Verb, HelpText = Help)]
    public class JekyllBuildOptions : IOptions
    {
        public const string Verb = "build";
        public const string Help = "Uses jekyll to build the actual flog.";
        public const string Welcome = "Running jekyll build";
    }
}
