using TheFipster.Aviation.Domain.OurAirports;

namespace TheFipster.Aviation.Domain
{
    public class FlightTerminator
    {
        public FlightTerminator() { }

        public FlightTerminator(string airportIcao, OurRunway? runway)
        {
            Airport = airportIcao;

            if (runway != null)
                Runway = runway.LeIdent + "/" + runway.HeIdent;
        }

        public string Airport { get; set; }
        public string? Runway { get; set; }
    }
}
