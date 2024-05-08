using TheFipster.Aviation.Domain.SimToolkitPro;

namespace TheFipster.Aviation.Domain
{
    public class LandingReport
    {
        public LandingReport() { }

        public LandingReport(Landing landing, Logbook flight, Fleet aircraft)
        {
            Landing = landing;
            Flight = flight;
            Aircraft = aircraft;
        }

        public Landing? Landing { get; set; }
        public Logbook? Flight { get; set; }
        public Fleet? Aircraft { get; set; }
    }
}
