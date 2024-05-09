using System.Text.Json;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.SimToolkitPro.Components
{
    public class SimToolkitProExportLoader
    {
        private readonly JsonSerializerOptions? options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Loads the given export file.
        /// </summary>
        /// <param name="exportFilepath">Filepath to the export file.</param>
        /// <returns>Parsed object <see cref="SimToolkitProFlight"/>.</returns>
        public IEnumerable<SimToolkitProFlight> Load(string exportFilepath)
        {
            var json = File.ReadAllText(exportFilepath);
            var export = JsonSerializer.Deserialize<SimToolkitProExport>(json, options);
            var flights = new List<SimToolkitProFlight>();

            foreach (var log in export.Logbook.OrderBy(x => x.LocalId))
            {
                var landing = export.Landings.FirstOrDefault(x => x.FlightId == log.LocalId);
                var flight = new SimToolkitProFlight(log, landing);
                flights.Add(flight);
            }

            return flights;
        }
    }
}