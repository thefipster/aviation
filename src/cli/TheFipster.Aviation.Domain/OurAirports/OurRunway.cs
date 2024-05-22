using System.Globalization;

namespace TheFipster.Aviation.Domain.OurAirports
{
    public class OurRunway : IOurAirportData
    {
        public int? HeDisplacedThreshold { get; set; }
        public double? HeHeadingDeg { get; set; }
        public int? HeElevationFeed { get; set; }
        public double? HeLongitude { get; set; }
        public double? HeLatitude { get; set; }
        public string? HeIdent { get; set; }
        public int? LeDisplacedThreshold { get; set; }
        public double? LeHeadingDeg { get; set; }
        public int? LeElevationFeed { get; set; }
        public double? LeLongitude { get; set; }
        public double? LeLatitude { get; set; }
        public string LeIdent { get; set; }
        public int Closed { get; set; }
        public int Lighted { get; set; }
        public string Surface { get; set; }
        public int? WidthFeet { get; set; }
        public int? LengthFeet { get; set; }
        public string AirportIdent { get; set; }
        public int AirportRef { get; set; }
        public int Id { get; set; }

        public static OurRunway FromOurAirportsCsv(string[] fields)
            => new OurRunway
            {
                Id = int.Parse(fields[0]),
                AirportRef = int.Parse(fields[1]),
                AirportIdent = fields[2],
                LengthFeet = string.IsNullOrEmpty(fields[3]) ? null : int.Parse(fields[3]),
                WidthFeet = string.IsNullOrEmpty(fields[4]) ? null : int.Parse(fields[4]),
                Surface = fields[5],
                Lighted = int.Parse(fields[6]),
                Closed = int.Parse(fields[7]),
                LeIdent = fields[8],
                LeLatitude = string.IsNullOrEmpty(fields[9]) ? null : double.Parse(fields[9], CultureInfo.InvariantCulture),
                LeLongitude = string.IsNullOrEmpty(fields[10]) ? null : double.Parse(fields[10], CultureInfo.InvariantCulture),
                LeElevationFeed = string.IsNullOrEmpty(fields[11]) ? null : int.Parse(fields[11]),
                LeHeadingDeg = string.IsNullOrEmpty(fields[12]) ? null : double.Parse(fields[12], CultureInfo.InvariantCulture),
                LeDisplacedThreshold = string.IsNullOrEmpty(fields[13]) ? null : int.Parse(fields[13]),
                HeIdent = fields[14],
                HeLatitude = string.IsNullOrEmpty(fields[15]) ? null : double.Parse(fields[15], CultureInfo.InvariantCulture),
                HeLongitude = string.IsNullOrEmpty(fields[16]) ? null : double.Parse(fields[16], CultureInfo.InvariantCulture),
                HeElevationFeed = string.IsNullOrEmpty(fields[17]) ? null : int.Parse(fields[17]),
                HeHeadingDeg = string.IsNullOrEmpty(fields[18]) ? null : double.Parse(fields[18], CultureInfo.InvariantCulture),
                HeDisplacedThreshold = string.IsNullOrEmpty(fields[19]) ? null : int.Parse(fields[19])
            };

        public override string ToString()
        {
            return $"{LeIdent}/{HeIdent} - {Surface} - {LengthFeet ?? -1}";
        }
    }
}
