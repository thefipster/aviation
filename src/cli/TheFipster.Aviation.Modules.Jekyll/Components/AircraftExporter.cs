using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Extensions;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Modules.Jekyll.Model;
using TheFipster.Aviation.Modules.Airports.Components;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    internal class AircraftExporter
    {
        internal string CreateFrontmatter(string flightsFolder, string airportFile)
        {
            var folders = Directory.GetDirectories(flightsFolder);

            var totalStats = new Stats();
            OurAirport lastAirport = null;
            var visitedCountries = new List<string>();
            var airports = new OurAirportFinder(new JsonReader<IEnumerable<OurAirport>>(), airportFile);

            foreach (var folder in folders)
            {
                var departureIcao = new FlightMeta().GetDeparture(folder);
                var departure = airports.SearchWithIcao(departureIcao);
                if (!visitedCountries.Contains(departure.IsoCountryCode))
                    visitedCountries.Add(departure.IsoCountryCode);

                var arrivalIcao = new FlightMeta().GetArrival(folder);
                var arrival = airports.SearchWithIcao(arrivalIcao);
                if (!visitedCountries.Contains(arrival.IsoCountryCode))
                    visitedCountries.Add(arrival.IsoCountryCode);

                var statsFile = new FlightFileScanner().GetFile(folder, FileTypes.StatsJson);
                var stats = new JsonReader<Stats>().FromFile(statsFile);
                processStats(totalStats, stats);

                lastAirport = arrival;
            }

            var aircraft = new Aircraft(folders.Count(), totalStats, lastAirport, visitedCountries);

            var frontmatter = new YamlWriter().ToYaml(aircraft);
            return frontmatter;
        }

        private void processStats(Stats total, Stats stats)
        {
            total.FuelUsed += stats.FuelUsed;
            total.FlightTime += stats.FlightTime;
            total.Passengers += stats.Passengers;

            if (stats.MaxGroundspeedMps > total.MaxGroundspeedMps)
                total.MaxGroundspeedMps = stats.MaxGroundspeedMps;

            if (stats.MaxAltitudeM > total.MaxAltitudeM)
                total.MaxAltitudeM = stats.MaxAltitudeM;
        }
    }
}
