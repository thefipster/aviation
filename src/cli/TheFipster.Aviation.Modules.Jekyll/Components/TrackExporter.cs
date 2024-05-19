using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Extensions;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Modules.Jekyll.Model;
using TheFipster.Aviation.Domain.Geo;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    internal class TrackExporter
    {
        public List<Track> GenerateCombinedTrack(string flightsFolder)
        {
            var combinedTrack = new List<Track>();

            var folders = Directory.GetDirectories(flightsFolder);
            foreach (var folder in folders)
            {
                if (folder.Contains("42"))
                    continue;

                var gpsFile = new FlightFileScanner().GetFile(folder, FileTypes.GpsJson);
                var gps = new JsonReader<GpsReport>().FromFile(gpsFile);

                var simbriefFile = new FlightFileScanner().GetFile(folder, FileTypes.SimbriefJson);
                var simbrief = new JsonReader<SimBriefFlight>().FromFile(simbriefFile);

                var track = gps.Coordinates.Select(x => new List<decimal>() { x.Latitude.RoundToSignificantDigits(5), x.Longitude.RoundToSignificantDigits(5) }).ToList();
                var name = gps.Departure + " - " + gps.Arrival;
                var uri = MetaInformation.GeneratePostUrl(simbrief);

                combinedTrack.Add(new Track(name, uri, track));
            }
            return combinedTrack;
        }
    }
}
