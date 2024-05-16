namespace TheFipster.Aviation.Domain
{
    public class GeoTagReport : JsonBase
    {
        public GeoTagReport()
        {
            GeoTags = new List<GeoTag>();
        }

        public GeoTagReport(string departure, string arrival)
            : this()
        {
            Departure = departure;
            Arrival = arrival;
        }

        public string Departure { get; set; }
        public string Arrival { get; set; }
        public ICollection<GeoTag> GeoTags { get; set; }
    }
}
