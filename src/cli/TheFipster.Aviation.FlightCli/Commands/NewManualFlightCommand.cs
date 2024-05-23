using TheFipster.Aviation.Domain;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.FlightCli.Services;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class NewManualFlightCommand : ICommand<NewManualFlightOptions>
    {
        public void Run(NewManualFlightOptions options, IConfig config)
        {
            Console.WriteLine(NewManualFlightOptions.Welcome);
            Guard.EnsureConfig(config);

            // initiate the flight with on of the hand crafted kml exports created with navigraph
            var leg = CommandRunner.ExecuteCommand<CreateNavigraphFlightCommand, CreateNavigraphFlightOptions, Leg>();

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
