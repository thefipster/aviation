using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.BlackBox
{
    public class BlackBoxTrimmer
    {
        public BlackBoxFlight Trim(BlackBoxFlight flight)
        {
            var trimmedFlight = new BlackBoxFlight(flight.Origin, flight.Destination);
            trimmedFlight.Records = flight.Records.Where(x => x.Engine1N2Percent > 0 && x.Engine2N2Percent > 0).ToList();
            return trimmedFlight;
        }
    }
}
