using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Datahub;

namespace TheFipster.Aviation.FlightApi.Models
{
    public class MapLeg
    {
        public MapLeg(Leg leg, Airport departure, Airport arrival, bool done)
        {
            No = leg.No;
            IsFlown = done;
            Departure = new Point(departure);
            Arrival = new Point(arrival);
        }

        public int No { get; set; }
        public Point Departure { get; set; }
        public Point Arrival { get; set; }
        public bool IsFlown { get; set; }
    }
}
