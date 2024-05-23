using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Modules.Jekyll.Model;
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
            var flightFile = new FlightFileScanner().GetFile(flightFolder, FileTypes.FlightJson);
            var flight = new JsonReader<FlightImport>().FromFile(flightFile);
            var airports = new OurAirportFinder(new JsonReader<IEnumerable<OurAirport>>(), airportFile);

            return GenerateFlightPost(flight, airports);  
        }

        internal Post GenerateFlightPost(FlightImport flight, OurAirportFinder airports)
        {
            var data = new FrontMatter(flight, airports);
            var frontmatter = new YamlWriter().ToFrontmatter(data);
            string name = MetaInformation.GeneratePostName(flight);

            return new Post(name, frontmatter);
        }
    }
}
