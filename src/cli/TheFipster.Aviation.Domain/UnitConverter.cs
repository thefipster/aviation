



namespace TheFipster.Aviation.Domain
{
    public static class UnitConverter
    {
        public static int FeetToMeters(int feet)
            => (int)(feet * 0.3048);

        public static double JetA1LiterToKg(int jetA1InLiters)
            => jetA1InLiters * 0.8;

        public static double KnotsToMetersPerSecond(int knots)
            => knots * 0.5144444444;

        public static double MetersToFeet(int meters)
            => meters * 3.280839895;

        public static double NauticalMilesToKilometers(int nauticalMiles)
            => nauticalMiles * 1.852;
    }
}
