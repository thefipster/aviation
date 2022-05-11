using System.Text.Json;
using TheFipster.Aviation.Domain.Datahub;

namespace TheFipster.Aviation.Modules.Airports
{
    public class Finder
    {
        private readonly IEnumerable<Airport> airports;

        public Finder()
        {
            var location = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var file = "airports.json";

            var path = Path.Combine(location, "Aviation", "data", file);

            if (!File.Exists(path))
                throw new ApplicationException("Couldn't locate airports.json file.");

            var json = File.ReadAllText(path);
            
            airports = JsonSerializer.Deserialize<IEnumerable<Airport>>(json);
        }

        public Airport? SearchWithIcao(string icao) => airports.FirstOrDefault(x => x.Ident == icao);
    }
}