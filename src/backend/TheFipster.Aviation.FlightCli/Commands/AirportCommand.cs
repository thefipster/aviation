using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Airports;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class AirportCommand
    {
        private HardcodedConfig config;

        public AirportCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(AirportOptions options)
        {
            var legs = new JsonReader<IEnumerable<Leg>>().FromFile(config.FlightPlanFile);

            var finder = new AirportFinder(config.AirportFile);

            foreach (var leg in legs)
            {
                var airport = finder.SearchWithIcao(leg.To.Trim());
                new JsonWriter<Airport>().Write(config.AirportFolder, airport, FileTypes.AirportJson, airport.Ident);
            }
        }
    }
}
