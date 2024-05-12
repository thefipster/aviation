using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.CoreCli.Abstractions
{
    public interface IFlightPlanWriter
    {
        void SaveFlightPlan(IEnumerable<Leg> flightPlan);
    }
}
