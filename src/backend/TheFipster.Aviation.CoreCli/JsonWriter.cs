using System.Text.Json;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.CoreCli
{
    public class JsonWriter<T> : IJsonWriter<T>
    {
        public void Write<T>(
            string flightFolder, 
            T data, 
            FileTypes filetype, 
            string? departure, 
            string? arrival = null, 
            bool overwrite = false) where T : JsonBase
        {
            var file = generateFilename(departure, arrival, filetype);
            var path = Path.Combine(flightFolder, file);
            Write(path, filetype, data, overwrite);
        }

        public void Write<T>(
            string filepath, 
            FileTypes filetype, 
            T data, 
            bool overwrite = false) where T : JsonBase
        {
            data.FileType = filetype;
            Write(filepath, data, overwrite);
        }

        public void Write<T>(
            string filepath, 
            T data, 
            bool overwrite = false)
        {
            if (File.Exists(filepath) && !overwrite)
                return;

            var json = JsonSerializer.Serialize(data);
            File.WriteAllText(filepath, json);
        }

        private static string generateFilename(
            string? departure, 
            string? arrival, 
            FileTypes filetype)
        {
            var filetypeName = getFiletypeName(filetype);
            return string.IsNullOrEmpty(arrival)
            ? $"{departure} - {filetypeName}.json"
                : $"{departure} - {arrival} - {filetypeName}.json";
        }

        private static string getFiletypeName(FileTypes filetype)
        {
            return filetype switch
            {
                FileTypes.AirportJson => "Airport",
                FileTypes.BlackBoxJson => "BlackBox",
                FileTypes.BlackBoxTrimmedJson => "BlackBoxTrimmed",
                FileTypes.BlackBoxCompressedJson => "BlackBoxCompressed",
                FileTypes.BlackBoxStatsJson => "BlackBoxStats",
                FileTypes.LandingJson => "Landing",
                FileTypes.LogbookJson => "Logbook",
                FileTypes.MergedFlightJson => "Flight",
                FileTypes.NotamJson => "Notams",
                FileTypes.SimbriefJson => "Simbrief",
                FileTypes.SimToolkitProJson => "SimToolkitPro",
                FileTypes.TrackJson => "Track",
                FileTypes.TrackCompressedJson => "TrackCompressed",
                FileTypes.RouteJson => "Route",
                FileTypes.WaypointsJson => "Waypoints",
                FileTypes.StatsJson => "Stats",
                FileTypes.GeoTagsJson => "GeoTags",
                FileTypes.GpsJson => "Gps",
                _ => throw new ApplicationException($"Unknown json file type {filetype}.")
            };
        }
    }
}
