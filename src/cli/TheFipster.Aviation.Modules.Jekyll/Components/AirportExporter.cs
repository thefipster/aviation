using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Modules.Jekyll.Model;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    internal class AirportExporter
    {
        public List<Location> GetPlannedAirports(string flightsFolder)
        {
            var airports = new List<Location>();
            var folders = Directory.GetDirectories(flightsFolder);
            foreach (var folder in folders)
            {
                var simbriefFile = new FlightFileScanner().GetFile(folder, FileTypes.SimbriefJson);
                var simbrief = new JsonReader<SimBriefFlight>().FromFile(simbriefFile);

                if (!airports.Any(x => x.Name == simbrief.Departure.Icao))
                    airports.Add(new Location(simbrief.Departure));

                if (!airports.Any(x => x.Name == simbrief.Arrival.Icao))
                    airports.Add(new Location(simbrief.Arrival));

                if (!airports.Any(x => x.Name == simbrief.Alternate.Icao))
                    airports.Add(new Location(simbrief.Alternate));
            }

            return airports;
        }

    }
}
