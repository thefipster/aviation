using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.Domain
{
    public class FlightBase : JsonBase
    {
        public FlightBase() : base() { }

        public FlightBase(FileTypes filetype, string departure, string arrival)
            : base(filetype)
        {
            Departure = departure;
            Arrival = arrival;
        }

        /// <summary>
        /// Planned Departure Airport ICAO.
        /// </summary>
        public string Departure { get; }

        /// <summary>
        /// Planned Arrival Airport ICAO.
        /// </summary>
        public string Arrival { get; }
    }
}
