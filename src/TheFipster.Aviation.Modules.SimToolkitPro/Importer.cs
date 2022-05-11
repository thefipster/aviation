using System.Text.Json;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.SimToolkitPro
{
    public class Importer
    {
        private JsonSerializerOptions? options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public void Read(string filepath)
        {
            var json = File.ReadAllText(filepath);
            var export = JsonSerializer.Deserialize<SimToolkitProExport>(json, options);

            foreach (var log in export.Logbook.Where(x => x.Status.ToLower() == "completed").OrderBy(x => x.LocalId))
            {
                var landing = export.Landings.FirstOrDefault(x => x.FlightId == log.LocalId);
                var flight = new SimToolkitProFlight(log, landing);

                new JsonWriter().Write(flight);
            }
        }
    }
}