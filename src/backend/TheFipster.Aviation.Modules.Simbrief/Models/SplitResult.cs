using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.Simbrief.Models
{
    public class SplitResult
    {
        public SimBriefFlight Flight { get; internal set; }
        public SimbriefWaypoints Waypoints { get; internal set; }
        public NotamReport Notams { get; internal set; }
        public string Ofp { get; internal set; }
    }
}
