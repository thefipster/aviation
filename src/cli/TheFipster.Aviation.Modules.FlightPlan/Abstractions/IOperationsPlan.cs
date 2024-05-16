using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.FlightPlan.Abstractions
{
    public interface IOperationsPlan
    {
        PlannedFlight GetNextFlight();
        Stats GetLastFlight();
    }
}
