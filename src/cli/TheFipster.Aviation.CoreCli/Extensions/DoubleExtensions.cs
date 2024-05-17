namespace TheFipster.Aviation.CoreCli.Extensions
{
    public static class DoubleExtensions
    {
        public static decimal RoundToSignificantDigits(this double d, int digits)
            => Math.Round(Convert.ToDecimal(d), digits);
    }
}
