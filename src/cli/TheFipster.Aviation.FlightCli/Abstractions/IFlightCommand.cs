namespace TheFipster.Aviation.FlightCli.Abstractions
{
    public interface IFlightCommand<T> : ICommand<T> where T : FlightOptions
    {
        void Run(T options, IConfig config);
    }
}
