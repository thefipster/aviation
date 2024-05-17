using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public interface IFlightRequiredCommand<T> where T: FlightRequiredOptions
    {
         void Run(T options, HardcodedConfig config);
    }
}
