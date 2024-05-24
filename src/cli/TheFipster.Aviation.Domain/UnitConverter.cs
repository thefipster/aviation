namespace TheFipster.Aviation.Domain
{
    public static class UnitConverter
    {
        // Distances
        public static int FtToM(int feet)
            => (int)(feet * 0.3048);

        public static double MToFt(int meters)
            => meters * 3.280839895;

        public static double NmToKm(int nauticalMiles)
            => nauticalMiles * 1.852;

        // Fuel Amount
        public static double JetA1LToKg(int jetA1InLiters)
            => jetA1InLiters * 0.8;

        // Velocity
        public static double KtsToMps(int knots)
            => knots * 0.5144444444;

        public static double MpsToKmh(int metersPerSecond)
            => metersPerSecond * 3.6;
    }
}
