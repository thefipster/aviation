using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class SimbriefImportCommand
    {
        internal void Run(SimbriefImportOptions options)
        {
            var flight = new Importer().Read(options.Filepath);
            new JsonWriter<SimBriefFlight>().Write(flight, "Simbrief", flight.Departure.Icao, flight.Arrival.Icao);
        }
    }
}
