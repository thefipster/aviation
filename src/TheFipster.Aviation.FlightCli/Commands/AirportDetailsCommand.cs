using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Airports;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class AirportDetailsCommand
    {
        internal void Run(AirportDetailsOptions options)
        {
            var airport = new Finder().SearchWithIcao(options.AirportIcaoCode);

            if (airport != null)
                new JsonWriter<Airport>().Write(airport, "Airport", airport.Ident);
            else
                throw new ApplicationException($"Airport with ICAO {options.AirportIcaoCode} was not found.");
        }
    }
}
