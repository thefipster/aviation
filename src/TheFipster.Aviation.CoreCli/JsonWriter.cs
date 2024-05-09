using Microsoft.VisualBasic.FileIO;
using System.Text.Json;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.CoreCli
{
    public class JsonWriter<T> where T: JsonBase
    {
        public void Write(string flightFolder, T data, FileTypes filetype, string? departure, string? arrival = null, bool overwrite = false)
        {
            data.FileType = filetype;

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

            var file = string.IsNullOrEmpty(arrival)
            ? $"{departure} - {filetypeName}.json"
                : $"{departure} - {arrival} - {filetypeName}.json";

            var path = Path.Combine(flightFolder, file);
            if (File.Exists(path) && !overwrite)
                return;

            var json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }
    }
}
