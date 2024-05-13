namespace TheFipster.Aviation.Domain
{
    public static class UnitConverter
    {
        public static int FeetToMeters(int feet)
            => (int)(feet * 0.3048);
    }
}
