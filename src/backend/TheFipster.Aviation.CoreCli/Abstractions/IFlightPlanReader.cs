using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.CoreCli.Abstractions
{
    public interface IFlightPlanReader
    {
        IEnumerable<Leg> GetFlightPlan();
    }
}
