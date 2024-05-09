using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Domain
{
    public class NotamReport : JsonBase
    {
        public NotamReport()
        {
            Notams = new List<Notam>();
        }

        public NotamReport(string departureIcao, string arrivalIcao, List<Notam> notams)
            : this()
        {
            Departure = departureIcao;
            Arrival = arrivalIcao;
            Notams = notams;
        }

        public string? Departure { get; set; }
        public string? Arrival { get; set; }
        public ICollection<Notam> Notams { get; set; }
    }
}
