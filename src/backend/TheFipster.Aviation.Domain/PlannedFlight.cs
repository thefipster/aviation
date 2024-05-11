using TheFipster.Aviation.Domain.Datahub;

namespace TheFipster.Aviation.Domain
{
    public class PlannedFlight
    {
        public PlannedFlight(int leg, Airport departure, Airport arrival)
        {
            Leg = leg;
            Departure = departure;
            Arrival = arrival;
        }

        public int Leg { get; set; }
        public Airport Departure { get; set; }
        public Airport Arrival { get; set; }
    }
}
