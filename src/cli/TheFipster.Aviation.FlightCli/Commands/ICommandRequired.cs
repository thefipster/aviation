using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public interface ICommandRequired<T> where T: DepArrRequiredOptions
    {
         void Run(T options, HardcodedConfig config);
    }
}
