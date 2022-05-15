using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Modules.Simbrief.Components;

namespace TheFipster.Aviation.Modules.Simbrief
{
    public class Importer
    {
        private readonly Loader loader;
        private readonly JsonWriter<SimBriefFlight> writer;

        public Importer()
        {
            loader = new Loader();
            writer = new JsonWriter<SimBriefFlight>();
        }

        public void Load(string folder)
        {
            var flight = loader.Read(folder);
            writer.Write(flight, "Simbrief", flight.Departure.Icao, flight.Arrival.Icao);
        }
    }
}
