using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Modules.Jekyll.Model;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Modules.Airports.Components;
using TheFipster.Aviation.Domain.OurAirports;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    public class FrontMatterPostExporter
    {
        public Post GenerateFlightPost(string flightFolder, string airportFile)
        {
            var gpsFile = new FlightFileScanner().GetFile(flightFolder, FileTypes.GpsJson);
            var gps = new JsonReader<GpsReport>().FromFile(gpsFile);

            var simbriefFile = new FlightFileScanner().GetFile(flightFolder, FileTypes.SimbriefJson);
            var simbrief = new JsonReader<SimBriefFlight>().FromFile(simbriefFile);

            var airports = new OurAirportFinder(new JsonReader<IEnumerable<OurAirport>>(), airportFile);

            string frontmatter = generateFrontmatter(simbrief, gps, airports);
            string name = MetaInformation.GeneratePostName(simbrief);

            return new Post(name, frontmatter);
        }

        private string generateFrontmatter(SimBriefFlight simbrief, GpsReport gps, OurAirportFinder airports)
        {
            var data = new FrontMatter(simbrief, airports);
            var frontmatter = convertYaml(data);
            return frontmatter;
        }

        private string convertYaml(FrontMatter data)
        {
            var serializer = new SerializerBuilder()
                            .WithNamingConvention(CamelCaseNamingConvention.Instance)
                            .Build();

            var yaml = serializer.Serialize(data);
            var frontmatter = Const.FrontmatterDelimiter
                + Environment.NewLine
                + yaml
                + Const.FrontmatterDelimiter
                + Environment.NewLine;
            return frontmatter;
        } 
    }
}
