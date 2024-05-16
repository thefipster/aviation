namespace TheFipster.Aviation.Domain.Enums
{
    public static class Continents
    {
        public static Dictionary<string, string> Dictionary => new Dictionary<string, string>
        {
            { "NA", "North America" },
            { "EU", "Europe" },
            { "AS", "Asia" },
            { "SA", "South America" },
            { "AF", "Africa" },
            { "OC", "Oceania" },
            { "AN", "Antarctica" }
        };
    }
}
