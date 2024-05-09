using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Datahub;

namespace TheFipster.Aviation.Modules.Merger
{
    public class AirportMerger
    {
        public List<Domain.Merged.Airport> Merge(IEnumerable<Airport> airports, SimBriefFlight simbrief)
        {
            var simbriefDeparture = simbrief.Departure;
            var datahubDeparture = airports.FirstOrDefault(x => x.Ident == simbriefDeparture.Icao);
            var departure = merge(simbriefDeparture, datahubDeparture, "Departure");

            var simbriefArrival = simbrief.Arrival;
            var datahubArrival = airports.FirstOrDefault(x => x.Ident == simbriefArrival.Icao);
            var arrival = merge(simbriefArrival, datahubArrival, "Arrival");

            var simbriefAlternate = simbrief.Alternate;
            var datahubAlternate = airports.FirstOrDefault(x => x.Ident == simbriefAlternate.Icao);
            var alternate = merge(simbriefAlternate, datahubAlternate, "Alternate");

            return new List<Domain.Merged.Airport> {departure, arrival, alternate};
        }

        private Domain.Merged.Airport merge(
            Domain.Simbrief.Airport simbrief, 
            Domain.Datahub.Airport datahub,
            string interaction)
        {
            var airport = new Domain.Merged.Airport();

            airport.Icao = simbrief.Icao;
            airport.Iata = datahub.IataCode;
            airport.Continent = datahub.Continent;
            airport.Name = datahub.Name;
            airport.Latitude = simbrief.Latitude;
            airport.Longitude = simbrief.Longitude;
            airport.Runway = simbrief.Runway;
            airport.Elevation = simbrief.Elevation;
            airport.Country = datahub.IsoCountry;
            airport.Municipality = datahub.Municipality;
            airport.Region = datahub.IsoRegion;
            airport.Type = datahub.Type;
            airport.TransitionAltitude = simbrief.TransitionAltitude;
            airport.TransitionLevel = simbrief.TransitionLevel;
            airport.Interaction = interaction;

            return airport;
        }
    }
}
