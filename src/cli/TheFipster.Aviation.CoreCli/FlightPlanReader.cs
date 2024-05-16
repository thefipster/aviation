using Microsoft.Extensions.Configuration;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.CoreCli
{
    public class FlightPlanReader : IFlightPlanReader
    {
        private readonly IJsonReader<IEnumerable<Leg>> reader;
        private string flightPlanFile;

        public FlightPlanReader(IJsonReader<IEnumerable<Leg>> reader, IConfiguration config)
        {
            this.reader = reader;

            try
            {
                flightPlanFile = config[ConfigKeys.FlightPlanFileLocationKey];
            }
            catch (NullReferenceException ex)
            {
                throw new ArgumentException($"Config is missing key {ConfigKeys.FlightPlanFileLocationKey}.", ex);
            }
        }

        public FlightPlanReader(IJsonReader<IEnumerable<Leg>> reader, string flightPlanFile)
        {
            this.reader = reader;
            this.flightPlanFile = flightPlanFile;
        }

        public IEnumerable<Leg> GetFlightPlan()
        {
            var legs = reader.FromFile(flightPlanFile);
            return legs;
        }
    }
}
