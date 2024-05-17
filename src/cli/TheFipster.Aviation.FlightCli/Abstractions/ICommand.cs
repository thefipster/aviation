namespace TheFipster.Aviation.FlightCli.Abstractions
{
    public interface ICommand<T> where T : IOptions
    {
        void Run(T options, IConfig config);
    }
}
