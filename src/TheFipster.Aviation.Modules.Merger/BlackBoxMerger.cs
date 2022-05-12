using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.Merger
{
    public class WaypointMerger
    {
        public IEnumerable<Domain.Merged.Waypoint> Merge(BlackBoxFlight blackbox)
        {
            var list = new List<Domain.Merged.Waypoint>();

            foreach (var point in blackbox.Records)
            {
                var merge = new Domain.Merged.Waypoint();

                merge.Timestamp = point.Timestamp;
                merge.AltimeterFeet = point.AltimeterFeet;
                merge.BankAngle = point.BankAngle;
                merge.BrakesActivated = point.BrakesActivated;
                merge.CompassHeadingRadians = point.CompassHeadingRadians;
                merge.ElevationMeters = point.ElevationMeters;
                merge.Engine1N1Percent = point.Engine1N1Percent;
                merge.Engine1N2Percent = point.Engine1N2Percent;
                merge.Engine2N1Percent = point.Engine2N1Percent;
                merge.Engine2N2Percent = point.Engine2N2Percent;
                merge.FlapsConfig = point.FlapsConfig;
                merge.FuelLiters = point.FuelLiters;
                merge.GearPosition = point.GearPosition;
                merge.GpsAltitudeMeters = point.GpsAltitudeMeters;
                merge.GroundSpeedMps = point.GroundSpeedMps;
                merge.IndicatedAirSpeedKnots = point.IndicatedAirSpeedKnots;
                merge.LatitudeDecimals = point.LatitudeDecimals;
                merge.LongitudeDecimals = point.LongitudeDecimals;
                merge.OnGroundFlag = point.OnGroundFlag;
                merge.OutsideAirTemperatureCelsius = point.OutsideAirTemperatureCelsius;
                merge.PitchAngle = point.PitchAngle;
                merge.RadioAltimeterMeters = point.RadioAltimeterMeters;
                merge.TotalAirTemperatureCelsius = point.TotalAirTemperatureCelsius;
                merge.TrueAirSpeedKnots = point.TrueAirSpeedKnots;
                merge.VerticalSpeedMps = point.VerticalSpeedMps;
                merge.WindDirectionRadians = point.WindDirectionRadians;
                merge.WindSpeedKnots = point.WindSpeedKnots;

                list.Add(merge);
            }

            return list;
        }
    }
}
