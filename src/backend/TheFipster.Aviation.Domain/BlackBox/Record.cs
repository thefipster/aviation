using System.Text.Json.Serialization;
using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Domain.BlackBox
{
    public class Record
    {
        [JsonPropertyName("t")]
        public long Timestamp { get; set; }

        
        [JsonPropertyName("ias")]
        public int IndicatedAirSpeedKnots { get; set; }
        
        [JsonPropertyName("tas")]
        public int TrueAirSpeedKnots { get; set; }
        
        [JsonPropertyName("gs")]
        public int GroundSpeedMps { get; set; }
        
        [JsonPropertyName("vs")]
        public int VerticalSpeedMps { get; set; }


        [JsonPropertyName("alt")]
        public int AltimeterFeet { get; set; }
        
        [JsonPropertyName("ra")]
        public int RadioAltimeterMeters { get; set; }
        
        [JsonPropertyName("ga")]
        public int GpsAltitudeMeters { get; set; }
        
        [JsonPropertyName("ele")]
        public int ElevationMeters { get; set; }


        [JsonPropertyName("lat")]
        public double LatitudeDecimals { get; set; }
        
        [JsonPropertyName("lon")]
        public double LongitudeDecimals { get; set; }


        [JsonPropertyName("ws")]
        public int WindSpeedKnots { get; set; }
        
        [JsonPropertyName("wd")]
        public int WindDirectionRadians { get; set; }


        [JsonPropertyName("oat")]
        public int OutsideAirTemperatureCelsius { get; set; }
        
        [JsonPropertyName("tat")]
        public int TotalAirTemperatureCelsius { get; set; }

        
        [JsonPropertyName("ba")]
        public double BankAngle { get; set; }
        
        [JsonPropertyName("pa")]
        public double PitchAngle { get; set; }
        
        [JsonPropertyName("hdg")]
        public double CompassHeadingRadians { get; set; }

        
        [JsonPropertyName("flp")]
        public string? FlapsConfig { get; set; }
        
        [JsonPropertyName("gr")]
        public string? GearPosition { get; set; }
        
        [JsonPropertyName("brk")]
        public string? BrakesActivated { get; set; }
        
        [JsonPropertyName("gnd")]
        public bool OnGroundFlag { get; set; }

        
        [JsonPropertyName("fl")]
        public int FuelLiters { get; set; }
        
        [JsonPropertyName("1n1")]
        public double Engine1N1Percent { get; set; }
        
        [JsonPropertyName("1n2")]
        public double Engine1N2Percent { get; set; }
        
        [JsonPropertyName("2n1")]
        public double Engine2N1Percent { get; set; }
        
        [JsonPropertyName("2n2")]
        public double Engine2N2Percent { get; set; }

        public void Print()
        {
            Console.Clear();
            Console.WriteLine($"IAS {IndicatedAirSpeedKnots} kts");
            Console.WriteLine($"TAS {TrueAirSpeedKnots} kts");
            Console.WriteLine($"GS {GroundSpeedMps} m/s");
            Console.WriteLine($"V/S {VerticalSpeedMps} m/s");
            Console.WriteLine();
            Console.WriteLine($"Elevation: {ElevationMeters} m");
            Console.WriteLine($"Altimeter: {AltimeterFeet} feet");
            Console.WriteLine($"Radio Altimeter: {RadioAltimeterMeters} m");
            Console.WriteLine($"GPS Altitude: {GpsAltitudeMeters} m");
            Console.WriteLine();
            Console.WriteLine($"Latitude: {LatitudeDecimals}");
            Console.WriteLine($"Longitude: {LongitudeDecimals}");
            Console.WriteLine();
            Console.WriteLine($"Wind: {WindSpeedKnots} kts");
            Console.WriteLine($"Direction: {WindDirectionRadians}°");
            Console.WriteLine();
            Console.WriteLine($"OAT: {OutsideAirTemperatureCelsius}°C");
            Console.WriteLine($"TAT: {TotalAirTemperatureCelsius}°C");
            Console.WriteLine();
            Console.WriteLine($"Bank: {BankAngle}°");
            Console.WriteLine($"Pitch: {PitchAngle}°");
            Console.WriteLine($"Heading: {CompassHeadingRadians}°");
            Console.WriteLine();
            Console.WriteLine($"Flaps: {FlapsConfig}");
            Console.WriteLine($"Gears: {GearPosition}");
            Console.WriteLine($"Brakes: {BrakesActivated}");
            Console.WriteLine($"On Ground: {OnGroundFlag}");
            Console.WriteLine();
            Console.WriteLine($"Fuel: {FuelLiters} l");
            Console.WriteLine();
            Console.WriteLine("Engine 1");
            Console.WriteLine($"N1: {Engine1N1Percent}%");
            Console.WriteLine($"N2: {Engine1N2Percent}%");
            Console.WriteLine();
            Console.WriteLine("Engine 2");
            Console.WriteLine($"N1: {Engine2N1Percent}%");
            Console.WriteLine($"N2: {Engine2N2Percent}%");
        }

        public override bool Equals(object? obj)
        {
            var coordinate = obj as Record;
            if (coordinate == null) return false;

            return coordinate.LatitudeDecimals == this.LatitudeDecimals
                && coordinate.LongitudeDecimals == this.LongitudeDecimals;
        }

        public override string ToString()
            => $"{LatitudeDecimals:0.00000} | {LongitudeDecimals:0.00000}";
    }
}
