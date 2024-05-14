using Microsoft.Extensions.Configuration;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.Modules.Airports.Abstractions;

namespace TheFipster.Aviation.Modules.Airports.Components
{
    public class AirportFinder : IAirportFinder
    {
        private readonly IEnumerable<Airport> airports;

        public AirportFinder(IJsonReader<IEnumerable<Airport>> reader, string airportFilepath)
            => airports = reader.FromFile(airportFilepath);

        public AirportFinder(IJsonReader<IEnumerable<Airport>> reader, IConfiguration config)
            : this(reader, config[ConfigKeys.AirportsFilepath]) { }

        public Airport SearchWithIcao(string icao) => airports.First(x => x.Ident == icao);
    }
}