using TheFipster.Aviation.Domain.BlackBox;

namespace TheFipster.Aviation.Domain
{
    public class BlackBoxFlight : JsonBase
    {
        public BlackBoxFlight()
            => Records = new List<Record>();

        public BlackBoxFlight(
            string origin,
            string destination)
            : this()
        {
            Origin = origin;
            Destination = destination;
        }

        public BlackBoxFlight(
            string origin,
            string destination,
            List<Record> coordinates)
            : this(origin, destination)
        {
            Records = coordinates;
        }

        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public ICollection<Record> Records { get; set; }
    }
}
