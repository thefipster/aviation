using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.SimToolkitPro;

namespace TheFipster.Aviation.Modules.SimToolkitPro.Components
{
    public class SimToolkitProScanner
    {
        public LogbookStats Scan(SimToolkitProFlight? simToolkitPro)
        {
            var stats = new LogbookStats();

            stats.FlightTime = 
                int.Parse(simToolkitPro.Logbook.ActualArr) 
                - int.Parse(simToolkitPro.Logbook.ActualDep);

            stats.FuelRamp = int.Parse(simToolkitPro.Logbook.FuelRamp);
            stats.FuelShutdown = int.Parse(simToolkitPro.Logbook.FuelShutdown);
            stats.FuelUsed = stats.FuelRamp - stats.FuelShutdown;

            return stats;
        }
    }
}
