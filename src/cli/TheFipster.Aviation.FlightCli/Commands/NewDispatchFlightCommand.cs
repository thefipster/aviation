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

            // initiate the flight by downloading the latest simbrief dispatch
            var leg = CommandRunner.ExecuteCommand<CreateSimbriefFlightCommand, CreateSimbriefFlightOptions, Leg>();

            // record the flight
            CommandRunner.ExecuteRequiredFlightCommand<BlackboxRecorderCommand, BlackboxRecorderOptions>(leg.From, leg.To);

            // get everything together
            CommandRunner.ExecuteRequiredFlightCommand<ImportCollectorCommand, ImportCollectorOptions>(leg.From, leg.To);
            CommandRunner.ExecuteGenericFlightCommand<ImportCombinerCommand, ImportCombinerOptions>(leg.From, leg.To);
            CommandRunner.ExecuteGenericFlightCommand<ImportProcessorCommand, ImportProcessorOptions>(leg.From, leg.To);

            // generate the flog
            CommandRunner.ExecuteGenericFlightCommand<JekyllCreateCommand, JekyllCreateOptions>(leg.From, leg.To);
            CommandRunner.ExecuteCommand<JekyllBuildCommand, JekyllBuildOptions>();
        }
    }
}
