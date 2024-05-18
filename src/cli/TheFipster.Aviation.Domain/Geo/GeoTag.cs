using System;
using TheFipster.Aviation.Domain.BlackBox;
using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Domain.Geo
{
    public class GeoTag
    {
        public GeoTag() { }

        public GeoTag(string filename, Record blackBoxRecord)
        {
            Screenshot = filename;
            Timestamp = blackBoxRecord.Timestamp;
            Latitude = blackBoxRecord.LatitudeDecimals;
            Longitude = blackBoxRecord.LongitudeDecimals;
            AltitudeInMeters = blackBoxRecord.GpsAltitudeMeters;
        }

        public GeoTag(string filename, long timestamp, List<double> stkpCoordinate)
        {
            Screenshot = filename;
            Timestamp = timestamp;
            var coordinate = Coordinate.FromToolkitCoordinate(stkpCoordinate);
            Latitude = coordinate.Latitude;
            Longitude = coordinate.Longitude;

            if (coordinate.Altitude.HasValue)
                AltitudeInMeters = UnitConverter.FeetToMeters(coordinate.Altitude.Value);
        }

        public string Screenshot { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? AltitudeInMeters { get; set; }
        public long Timestamp { get; set; }
    }
}
