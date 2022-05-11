using TheFipster.Aviation.Domain.BlackBox;

namespace TheFipster.Aviation.Domain
{
    public class BlackBoxFlight
    {
        public BlackBoxFlight()
        {
            Records = new List<Record>();
        }

        public BlackBoxFlight(string origin, string destination)
        : this()
        {
            Origin = origin;
            Destination = destination;
        }

        public string Origin { get; set; }
        public string Destination { get; set; }

        public ICollection<Record> Records { get; set; }
    }
}
