namespace TheFipster.Aviation.FlightCli.Abstractions
{
    public interface IFlightRequiredCommand<T> : ICommand<T> where T : FlightRequiredOptions
    {
        void Run(T options, IConfig config);
    }
}
