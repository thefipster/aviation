using TheFipster.Aviation.Domain.Datahub;

namespace TheFipster.Aviation.Modules.Airports.Abstractions
{
    public interface IAirportFinder
    {
        Airport? SearchWithIcao(string icao);
    }
}
