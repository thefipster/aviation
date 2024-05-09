using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.Domain.Merged
{
    public class Record
    {
        public long Timestamp { get; set; }
        public int AltimeterFeet { get; set; }
        public double BankAngle { get; set; }
        public string BrakesActivated { get; set; }
        public double CompassHeadingRadians { get; set; }
        public int ElevationMeters { get; set; }
        public double Engine1N1Percent { get; set; }
        public double Engine1N2Percent { get; set; }
        public double Engine2N1Percent { get; set; }
        public double Engine2N2Percent { get; set; }
        public string FlapsConfig { get; set; }
        public int FuelLiters { get; set; }
        public string GearPosition { get; set; }
        public int GpsAltitudeMeters { get; set; }
        public int GroundSpeedMps { get; set; }
        public int IndicatedAirSpeedKnots { get; set; }
        public double LatitudeDecimals { get; set; }
        public double LongitudeDecimals { get; set; }
        public bool OnGroundFlag { get; set; }
        public int OutsideAirTemperatureCelsius { get; set; }
        public double PitchAngle { get; set; }
        public int RadioAltimeterMeters { get; set; }
        public int TotalAirTemperatureCelsius { get; set; }
        public int TrueAirSpeedKnots { get; set; }
        public int VerticalSpeedMps { get; set; }
        public int WindDirectionRadians { get; set; }
        public int WindSpeedKnots { get; set; }
    }
}
