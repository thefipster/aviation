using System.Text.Json;
using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.CoreCli
{
    public class JsonWriter<T>
    {
        public void Write(T data, string fileType, string from, string? to = null, bool overwrite = false)
        {
            var location = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            Write(location, data, fileType, from, to, overwrite);
        }

        public void Write(string flightPath, T data, string fileType, string from, string? to = null, bool overwrite = false)
        {
            var file = string.IsNullOrEmpty(to)
                ? $"{@from} - {fileType}.json"
                : $"{@from} - {to} - {fileType}.json";

            Write(flightPath, data, file, overwrite);
        }

        public void Write(string flightPath, T data, string filename, bool overwrite = false)
        {
            var path = Path.Combine(flightPath, filename);

            if (File.Exists(path) && !overwrite)
                return;

            var json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }

        public void Write(string flightFolder, T data, FileTypes filetype, string? dep, string? arr, bool overwrite = false)
        {
            string filetypeName = filetype switch
            {
                FileTypes.AirportJson => "Airport",
                FileTypes.BlackBoxJson => "BlackBox",
                FileTypes.LandingJson => "Landing",
                FileTypes.LogbookJson => "Logbook",
                FileTypes.MergedFlightJson => "Flight",
                FileTypes.NotamJson => "Notams",
                FileTypes.SimbriefJson => "Simbrief",
                FileTypes.SimToolkitProJson => "SimToolkitPro",
                FileTypes.TrackJson => "Track",
                FileTypes.RouteJson => "Route",
                FileTypes.WaypointsJson => "Waypoints",
                _ => throw new ApplicationException($"Unknown json file type {filetype}.")
            } ;

            Write(flightFolder, data, filetypeName, dep, arr, overwrite);
        }
    }
}
