using System.ComponentModel;

namespace TheFipster.Aviation.CoreCli
{
    public static class GpsCalculator
    {
        public static double GetHaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371;
            var φ1 = lat1.toRadians();
            var φ2 = lat2.toRadians();
            var Δφ = (lat2 - lat1).toRadians();
            var Δλ = (lon2 - lon1).toRadians();

            var a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
                    Math.Cos(φ1) * Math.Cos(φ2) *
                    Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var d = R * c;
            return d;
        }

        public static double GetBearing(double lat1, double lon1, double lat2, double lon2)
        {
            double x = Math.Cos(DegreesToRadians(lat1)) * Math.Sin(DegreesToRadians(lat2)) 
                - Math.Sin(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) * Math.Cos(DegreesToRadians(lon2 - lon1));
            double y = Math.Sin(DegreesToRadians(lon2 - lon1)) * Math.Cos(DegreesToRadians(lat2));
            var brng = Math.Atan2(y, x);
            return (RadiansToDegrees(brng) + 360) % 360;
        }

        public static double DegreesToRadians(double angleInDegrees)
        {
            return angleInDegrees * Math.PI / 180.0d;
        }

        public static double RadiansToDegrees(double angleInRadians)
        {
            return 180.0d * angleInRadians / Math.PI;
        }

        static double toRadians(this double angle)
        {
            return (angle * Math.PI) / 180;
        }
    }
}
