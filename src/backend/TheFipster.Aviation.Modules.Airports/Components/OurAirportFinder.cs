using Microsoft.Extensions.Configuration;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.OurAirports;

namespace TheFipster.Aviation.Modules.Airports.Components
{
    public class OurAirportFinder
    {
        private readonly IEnumerable<OurAirport> airports;

        public OurAirportFinder(IJsonReader<IEnumerable<OurAirport>> reader, string airportFilepath)
            => airports = reader.FromFile(airportFilepath);

        public OurAirportFinder(IJsonReader<IEnumerable<OurAirport>> reader, IConfiguration config)
            : this(reader, config[ConfigKeys.AirportsFilepath]) { }

        public OurAirport SearchWithIcao(string icao) => airports.First(x => x.Ident == icao);
    }
}