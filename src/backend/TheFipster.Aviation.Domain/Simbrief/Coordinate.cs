using System.Globalization;
using TheFipster.Aviation.Domain.BlackBox;

namespace TheFipster.Aviation.Domain.Simbrief
{
    public class Coordinate
    {
        public Coordinate()
        {

        }

        public Coordinate(Record x)
        {
            Latitude = x.LatitudeDecimals;
            Longitude = x.LongitudeDecimals;
            Altitude = x.GpsAltitudeMeters;
        }

        public Coordinate(List<double> point)
        {
            Latitude = point[1];
            Longitude = point[0];

            if (point.Count > 2)
                Altitude = UnitConverter.FeetToMeters((int)point[2]);
        }

        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Coordinate(double latitude, double longitude, int? altitude)
            : this(latitude, longitude)
        {
            Altitude = altitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? Altitude { get; set; }

        public static Coordinate FromToolkitCoordinateString(string stkpCoordinate)
        {
            var split = stkpCoordinate.Split(',');
            int? altitude = null;
            var latitude = double.Parse(split[1].Trim(), CultureInfo.InvariantCulture);
            var longitude = double.Parse(split[1].Trim(), CultureInfo.InvariantCulture);
            if (split.Length > 2)
                altitude = int.Parse(split[2]);

            return new Coordinate(latitude, longitude, altitude);
        }

        internal static Coordinate FromToolkitCoordinate(List<double> stkpCoordinate)
        {
            int? altitude = null;
            var latitude = stkpCoordinate[1];
            var longitude = stkpCoordinate[0];
            if (stkpCoordinate.Count > 2)
                altitude = (int)stkpCoordinate[2];

            return new Coordinate(latitude, longitude, altitude);
        }

        public override bool Equals(object? obj)
        {
            var coordinate = obj as Coordinate;
            if (coordinate == null) return false;

            return coordinate.Latitude == this.Latitude 
                && coordinate.Longitude == this.Longitude 
                && coordinate.Altitude == this.Altitude;
        }

        public override string ToString()
        {
            if (Altitude.HasValue)
                return $"{Latitude} | {Longitude} | {Altitude}";

                return $"{Latitude} | {Longitude}";
        }
    }
}
