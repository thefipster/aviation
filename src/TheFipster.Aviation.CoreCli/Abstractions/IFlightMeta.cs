namespace TheFipster.Aviation.CoreCli.Abstractions
{
    public interface IFlightMeta
    {
        int GetLeg(string flightFolder);
        string GetDeparture(string flightFolder);
        string GetArrival(string flightFolder);
    }
}
