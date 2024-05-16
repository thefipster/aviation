using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public interface ICommand<T> where T: DepArrOptions
    {
         void Run(T options, HardcodedConfig config);
    }
}
