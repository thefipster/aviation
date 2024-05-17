using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.CoreCli;

namespace TheFipster.Aviation.Modules.BlackBox
{
    public class BlackBoxScanner
    {
        public BlackBoxStats GenerateStatsFromBlackbox(string folder)
        {
            var blackboxFile = new FlightFileScanner().GetFile(folder, FileTypes.BlackBoxJson);
            var blackbox = new JsonReader<BlackBoxFlight>().FromFile(blackboxFile);

            var records = blackbox.Records.ToList();
            var stats = new BlackBoxStats(blackbox.Origin, blackbox.Destination);

            bool above = true;
            bool below = true;

            for (int i = 1; i < records.Count; i++)
            {
                var last = records[i - 1];
                var cur = records[i];

                if (last.FlapsConfig != cur.FlapsConfig)
                {
                    var waypoint = new Waypoint($"Flaps {last.FlapsConfig}->{cur.FlapsConfig}", cur.LatitudeDecimals, cur.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                }

                if (last.GearPosition != cur.GearPosition)
                {
                    var waypoint = new Waypoint($"Gear {cur.GearPosition}", cur.LatitudeDecimals, cur.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                }

                if (last.OnGroundFlag != cur.OnGroundFlag)
                {
                    var waypoint = new Waypoint(cur.OnGroundFlag ? "Landing" : "Takeoff", cur.LatitudeDecimals, cur.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                }

                if (above && last.AltimeterFeet < 10000 && cur.AltimeterFeet >= 10000)
                {
                    var waypoint = new Waypoint("Above 10.000 feet", cur.LatitudeDecimals, cur.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    above = false;
                }

                if (below && last.AltimeterFeet >= 10000 && cur.AltimeterFeet < 10000)
                {
                    var waypoint = new Waypoint("Below 10.000 feet", cur.LatitudeDecimals, cur.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    below = false;
                }

                var trip = GpsCalculator.GetHaversineDistance(last.LatitudeDecimals, last.LongitudeDecimals, cur.LatitudeDecimals, cur.LongitudeDecimals);
                if (last.OnGroundFlag)
                    stats.DistanceGround += trip;
                else
                    stats.DistanceAir += trip;

                if (cur.AltimeterFeet < 10000 && cur.IndicatedAirSpeedKnots > 250)
                    stats.Below10000SpeedWarning = true;

                if (cur.GpsAltitudeMeters > stats.MaxAltitudeM)
                    stats.MaxAltitudeM = cur.GpsAltitudeMeters;

                if (cur.GroundSpeedMps > stats.MaxGroundSpeedMps)
                    stats.MaxGroundSpeedMps = cur.GroundSpeedMps;

                if (cur.VerticalSpeedMps > stats.MaxClimbMps)
                    stats.MaxClimbMps = cur.VerticalSpeedMps;

                if (cur.VerticalSpeedMps < stats.MaxDescentMps)
                    stats.MaxDescentMps = cur.VerticalSpeedMps;

                var windspeed = UnitConverter.KnotsToMetersPerSecond(cur.WindSpeedKnots);
                if (windspeed > stats.MaxWindspeed)
                {
                    stats.MaxWindspeed = windspeed;
                    stats.WindDirectionRad = cur.WindDirectionRadians;
                }
            }

            bool tocTrigger = true;
            bool gsTrigger = true;
            bool climbTrigger = true;
            bool descentTrigger = true;
            bool windTrigger = true;

            for (int i = 0; i < records.Count; i++)
            {
                var rec = records[i];
                if (tocTrigger && stats.MaxAltitudeM * 0.999 < rec.GpsAltitudeMeters)
                {
                    var waypoint = new Waypoint($"Real TOC", rec.LatitudeDecimals, rec.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    tocTrigger = false;
                }

                if (gsTrigger && rec.GroundSpeedMps == stats.MaxGroundSpeedMps)
                {
                    var waypoint = new Waypoint($"Max GS", rec.LatitudeDecimals, rec.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    gsTrigger = false;
                }

                if (climbTrigger && rec.VerticalSpeedMps == stats.MaxClimbMps)
                {
                    var waypoint = new Waypoint($"Max Climb", rec.LatitudeDecimals, rec.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    climbTrigger = false;
                }

                if (descentTrigger && rec.VerticalSpeedMps == stats.MaxDescentMps)
                {
                    var waypoint = new Waypoint($"Max Descent", rec.LatitudeDecimals, rec.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    descentTrigger = false;
                }

                var windspeed = UnitConverter.KnotsToMetersPerSecond(rec.WindSpeedKnots);
                if (windTrigger && windspeed == stats.MaxWindspeed)
                {
                    var waypoint = new Waypoint($"Max Wind", rec.LatitudeDecimals, rec.LongitudeDecimals);
                    stats.Waypoints.Add(waypoint);
                    windTrigger = false;
                }
            }

            return stats;
        }
    }
}
