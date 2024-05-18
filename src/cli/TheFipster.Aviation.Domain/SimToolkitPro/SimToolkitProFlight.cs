using TheFipster.Aviation.Domain.SimToolkitPro;

namespace TheFipster.Aviation.Domain
{
    public class SimToolkitProFlight : JsonBase
    {
        public SimToolkitProFlight() 
        {
            Logbook = new Logbook();
        }

        public SimToolkitProFlight(Logbook log, Landing? landing)
        {
            Logbook = log;
            Landing = landing;
        }

        public Logbook Logbook { get; set; }
        public Landing? Landing { get; set; }
    }
}
