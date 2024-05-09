namespace TheFipster.Aviation.Domain.Simbrief
{
    public class Airport
    {
        public string Icao { get; set; }
        public int Elevation { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public string Runway { get; set; }
        public int TransitionAltitude { get; set; }
        public int TransitionLevel { get; set; }
        public string? Metar { get; set; }
        public string? Taf { get; set; }
    }
}
