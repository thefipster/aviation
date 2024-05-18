using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.SimToolkitPro;

namespace TheFipster.Aviation.Modules.SimToolkitPro.Components
{
    public class SimToolkitProTrackExtracter
    {
        public Track FromText(string trackedGeoJson, string departure, string arrival)
        {
            Track? track = FromText(trackedGeoJson);
            track.Departure = departure;
            track.Arrival = arrival;

            return track;
        }

        public Track FromText(string trackedGeoJson)
            => new JsonReader<Track>().FromText(trackedGeoJson);
    }
}
