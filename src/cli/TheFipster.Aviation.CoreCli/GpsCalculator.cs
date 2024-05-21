namespace TheFipster.Aviation.CoreCli
{
    public static class GpsCalculator
    {
        public const double EarthRadiusKm = 6371;

        /// <summary>
        /// Calculates an approximate distance between latlon1 and latlon2 in kilometers.
        /// </summary>
        /// <param name="lat1">Latitude point 1</param>
        /// <param name="lon1">Longitude point 1</param>
        /// <param name="lat2">Latitude point 2</param>
        /// <param name="lon2">Longitude point 2</param>
        /// <returns>Distance between latlon1 and latlon2 in kilometers.</returns>
        public static double GetHaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var lat1Rads = lat1.toRadians();
            var lat2Rads = lat2.toRadians();
            var dLatRads = (lat2 - lat1).toRadians();
            var dLonRads = (lon2 - lon1).toRadians();

            var x = Math.Sin(dLatRads / 2) 
                * Math.Sin(dLatRads / 2) 
                + Math.Cos(lat1Rads) 
                * Math.Cos(lat2Rads) 
                * Math.Sin(dLonRads / 2) 
                * Math.Sin(dLonRads / 2);

            var y = 2 * Math.Atan2(
                Math.Sqrt(x), 
                Math.Sqrt(1 - x));

            var distance = EarthRadiusKm * y;
            return distance;
        }

        /// <summary>
        /// Calculates the bearing to get from point latlon1 to point latlon2.
        /// </summary>
        /// <param name="lat1">Latitude point 1</param>
        /// <param name="lon1">Longitude point 1</param>
        /// <param name="lat2">Latitude point 2</param>
        /// <param name="lon2">Longitude point 2</param>
        /// <returns>Bearing from latlon1 to latlon2 is degrees</returns>
        public static double GetBearing(double lat1, double lon1, double lat2, double lon2)
        {
            double x = Math.Cos(lat1.toRadians()) 
                * Math.Sin(lat2.toRadians()) 
                - Math.Sin(lat1.toRadians()) 
                * Math.Cos(lat2.toRadians()) 
                * Math.Cos((lon2 - lon1).toRadians());

            double y = Math.Sin((lon2 - lon1).toRadians()) 
                * Math.Cos(lat2.toRadians());

            var bearing = Math.Atan2(y, x);
            return (bearing.toDegree() + 360) % 360;
        }

        /// <summary>
        /// Calculates the angle in degrees where the lines from latlon1 to latlon2 and latlon2 to latlon3 intersect.
        /// </summary>
        /// <param name="lat1">Latitude point 1</param>
        /// <param name="lon1">Longitude point 1</param>
        /// <param name="lat2">Latitude point 2</param>
        /// <param name="lon2">Longitude point 2</param>
        /// <param name="lat2">Latitude point 3</param>
        /// <param name="lon2">Longitude point 3</param>
        /// <returns>Angle in degrees of the intersection between the lines from latlon1 to latlon2 and latlon2 to latlon3.</returns>
        public static double GetAngle(double lat1, double lon1, double lat2, double lon2, double lat3, double lon3)
        {
            var bearing1 = GetBearing(lat1, lon1, lat2, lon2);
            var bearing2 = GetBearing(lat2, lon2, lat3, lon3);

            var angle = Math.Abs(bearing1 - bearing2);
            return angle;
        }

        /// <summary>
        /// Calculates the shortest distance from a line between latlon3 and latlon3 to a point latlon3
        /// </summary>
        /// <param name="lat1">Latitude linestart</param>
        /// <param name="lon1">Longitude linestart</param>
        /// <param name="lat2">Latitude lineend</param>
        /// <param name="lon2">Longitude lineend</param>
        /// <param name="lat2">Latitude point 3</param>
        /// <param name="lon2">Longitude point 3</param>
        /// <returns>Shortest distance between line and point in kilometers.</returns>
        public static double GetCrossTrack(double lat1, double lon1, double lat2, double lon2, double lat3, double lon3)
        {
            var bearing1 = GetBearing(lat1, lon1, lat3, lon3);
            bearing1 = 360 - ((bearing1 + 360) % 360);

            var bearing2 = GetBearing(lat1, lon1, lat2, lon2);
            bearing2 = 360 - ((bearing2 + 360) % 360);

            double lat1Rads = lat1.toRadians();
            double lat3Rads = lat3.toRadians();
            double dLon = (lon3 - lon1).toRadians();

            double distanceAC = Math.Acos(
                Math.Sin(lat1Rads) 
                * Math.Sin(lat3Rads) 
                + Math.Cos(lat1Rads) 
                * Math.Cos(lat3Rads) 
                * Math.Cos(dLon)
                ) 
                * EarthRadiusKm;

            double min_distance = Math.Abs(
                Math.Asin(
                    Math.Sin(distanceAC / 6371) 
                    * Math.Sin(bearing1.toRadians() 
                    - bearing2.toRadians())) 
                * EarthRadiusKm);

            return min_distance;
        }

        /// <summary>
        /// Converts the given angle from degrees to radians.
        /// </summary>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>Angle in radians</returns>
        public static double toRadians(this double angle)
            => angle * Math.PI / 180;


        /// <summary>
        /// Converts the given angle from radians to degrees.
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        /// <returns>Angle in degrees</returns>
        public static double toDegree(this double angle)
            => angle * 180.0d / Math.PI;
    }
}
