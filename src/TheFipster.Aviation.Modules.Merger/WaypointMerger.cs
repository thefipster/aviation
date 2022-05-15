using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.Merger
{
    public class WaypointMerger
    {
        public IEnumerable<Domain.Merged.Waypoint> Merge(SimBriefFlight simbrief)
        {
            var waypoints = new List<Domain.Merged.Waypoint>();

            foreach (var point in simbrief.Waypoints)
            {
                var waypoint = new Domain.Merged.Waypoint();
                waypoint.Index = point.Index;
                waypoint.Name = point.Name;
                waypoint.Latitude = point.Latitude;
                waypoint.Airway = point.Airway;
                waypoint.Longitude = point.Longitude;
                waypoints.Add(waypoint);
            }

            return waypoints;
        }
    }
}
