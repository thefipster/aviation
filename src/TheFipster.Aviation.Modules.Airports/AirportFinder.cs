using System.Text.Json;
using TheFipster.Aviation.Domain.Datahub;

namespace TheFipster.Aviation.Modules.Airports
{
    public class AirportFinder
    {
        private readonly IEnumerable<Airport> airports;

        public AirportFinder(string airportFile)
        {
            var json = File.ReadAllText(airportFile);
            airports = JsonSerializer.Deserialize<IEnumerable<Airport>>(json);
        }

        public Airport? SearchWithIcao(string icao) => airports.FirstOrDefault(x => x.Ident == icao);
    }
}