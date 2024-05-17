using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public interface IFlightCommand<T> where T: FlightOptions
    {
         void Run(T options, IConfig config);
    }
}
