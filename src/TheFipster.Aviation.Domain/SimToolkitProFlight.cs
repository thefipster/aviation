using TheFipster.Aviation.Domain.SimToolkitPro;

namespace TheFipster.Aviation.Domain
{
    public class SimToolkitProFlight
    {
        public SimToolkitProFlight()
        {

        }

        public SimToolkitProFlight(Logbook log, Landing? landing)
        {
            Logbook = log;
            Landing = landing;
        }

        public Logbook? Logbook { get; set; }
        public Landing? Landing { get; set; }
    }
}
