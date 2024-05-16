using Microsoft.Extensions.Configuration;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.CoreCli
{
    public class FlightPlanWriter : IFlightPlanWriter
    {
        private readonly IJsonWriter<IEnumerable<Leg>> writer;
        private string flightPlanFile;

        public FlightPlanWriter(IJsonWriter<IEnumerable<Leg>> writer, IConfiguration config)
        {
            this.writer = writer;
            flightPlanFile = config[ConfigKeys.FlightPlanFileLocationKey];
        }

        public FlightPlanWriter(IJsonWriter<IEnumerable<Leg>> writer, string flightPlanFile)
        {
            this.writer = writer;
            this.flightPlanFile = flightPlanFile;
        }

        public void SaveFlightPlan(IEnumerable<Leg> flightPlan)
            => writer.Write(flightPlanFile, flightPlan, true);
    }
}
