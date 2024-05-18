using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Domain
{
    public class BlackBoxStats : JsonBase
    {
        public BlackBoxStats()
        {
            Waypoints = new List<Waypoint>();
        }

        public BlackBoxStats(string? origin, string? destination)
            : this()
        {
            Departure = origin;
            Arrival = destination;
        }

        public string? Departure { get; set; }
        public string? Arrival { get; set; }
        public int MaxAltitudeM { get; set; }
        public int MaxGroundSpeedMps { get; set; }
        public int MaxClimbMps { get; set; }
        public int MaxDescentMps { get; set; }
        public double MaxWindspeed { get; set; }
        public int WindDirectionRad { get; set; }
        public ICollection<Waypoint> Waypoints { get; set; }
        public bool Below10000SpeedWarning { get; set; }
        public double DistanceGround { get; set; }
        public double DistanceAir { get; set; }
    }
}
