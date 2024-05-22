using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Services
{
    internal static class CommandRunner
    {
        internal static void ExecuteCommand<Tc, To>()
            where Tc : ICommand<To>, new()
            where To : IOptions, new()
        {
            var command = new Tc();
            var options = new To();

            command.Run(options, new HardcodedConfig());
        }

        internal static Tr ExecuteCommand<Tc, To, Tr>()
            where Tc : ICommand<To, Tr>, new()
            where To : IOptions, new()
        {
            var command = new Tc();
            var options = new To();

            var result = command.Run(options, new HardcodedConfig());
            return result;
        }

        internal static void ExecuteGenericFlightCommand<Tc, To>(string? departure, string? arrival)
            where Tc : IFlightCommand<To>, new()
            where To : FlightOptions, new()
        {
            var command = new Tc();
            var options = new To();

            options.DepartureAirport = departure;
            options.ArrivalAirport = arrival;

            command.Run(options, new HardcodedConfig());
        }

        internal static void ExecuteRequiredFlightCommand<Tc, To>(string departure, string arrival)
            where Tc : IFlightRequiredCommand<To>, new()
            where To : FlightRequiredOptions, new()
        {
            var command = new Tc();
            var options = new To();

            options.DepartureAirport = departure;
            options.ArrivalAirport = arrival;

            command.Run(options, new HardcodedConfig());
        }
    }
}
