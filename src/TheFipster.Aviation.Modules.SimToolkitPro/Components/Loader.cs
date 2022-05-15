using System.Text.Json;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.SimToolkitPro.Components
{
    public class Loader
    {
        private readonly JsonSerializerOptions? options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public IEnumerable<SimToolkitProFlight> Read(string filepath)
        {
            var json = File.ReadAllText(filepath);
            var export = JsonSerializer.Deserialize<SimToolkitProExport>(json, options);
            var flights = new List<SimToolkitProFlight>();

            foreach (var log in export.Logbook.Where(x => x.Status.ToLower() == "completed")
                                              .OrderBy(x => x.LocalId))
            {
                var landing = export.Landings.FirstOrDefault(x => x.FlightId == log.LocalId);
                var flight = new SimToolkitProFlight(log, landing);
                flights.Add(flight);
            }

            return flights;
        }
    }
}