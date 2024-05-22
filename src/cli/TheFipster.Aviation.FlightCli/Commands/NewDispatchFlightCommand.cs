using TheFipster.Aviation.Domain;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.FlightCli.Services;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class NewDispatchFlightCommand : ICommand<NewDispatchFlightOptions>
    {
        public void Run(NewDispatchFlightOptions options, IConfig config)
        {
            Console.WriteLine(NewDispatchFlightOptions.Welcome);
            Guard.EnsureConfig(config);

            var leg = CommandRunner.ExecuteCommand<CreateSimbriefFlightCommand, CreateSimbriefFlightOptions, Leg>();
            CommandRunner.ExecuteRequiredFlightCommand<BlackboxRecorderCommand, BlackboxRecorderOptions>(leg.From, leg.To);
            CommandRunner.ExecuteRequiredFlightCommand<ImportCollectorCommand, ImportCollectorOptions>(leg.From, leg.To);
            CommandRunner.ExecuteGenericFlightCommand<ImportCombinerCommand, ImportCombinerOptions>(leg.From, leg.To);
            CommandRunner.ExecuteGenericFlightCommand<ImportProcessorCommand, ImportProcessorOptions>(leg.From, leg.To);
        }
    }
}
