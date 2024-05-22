namespace TheFipster.Aviation.FlightCli.Abstractions
{
    public interface ICommand<T> where T : IOptions
    {
        void Run(T options, IConfig config);
    }

    public interface ICommand<T, Tr> where T : IOptions
    {
        Tr Run(T options, IConfig config);
    }
}
