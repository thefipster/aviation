using System.Text.Json;
using TheFipster.Aviation.Domain.Datahub;

namespace TheFipster.Aviation.Modules.Airports
{
    public class Finder
    {
        private const string DatabaseFile = "airport-codes.json";
        private readonly IEnumerable<Airport> airports;

        public Finder()
        {
            var location = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var path = Path.Combine(location, "Aviation", "data", DatabaseFile);

            if (!File.Exists(path))
                throw new ApplicationException("Couldn't locate airports.json file.");

            var json = File.ReadAllText(path);

            airports = JsonSerializer.Deserialize<IEnumerable<Airport>>(json);
        }

        public Finder(string airportFile)
        {
            var json = File.ReadAllText(airportFile);

            airports = JsonSerializer.Deserialize<IEnumerable<Airport>>(json);
        }

        public Airport? SearchWithIcao(string icao) => airports.FirstOrDefault(x => x.Ident == icao);
    }
}