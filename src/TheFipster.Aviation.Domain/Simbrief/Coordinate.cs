namespace TheFipster.Aviation.Domain.Simbrief
{
    public class Coordinate
    {
        public Coordinate()
        {

        }

        public Coordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Coordinate(double latitude, double longitude, int altitude)
            : this(latitude, longitude)
        {
            Altitude = altitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? Altitude { get; set; }
    }
}
