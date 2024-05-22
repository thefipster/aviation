using Microsoft.Extensions.Configuration;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Modules.Airports.Components
{
    public class OurAirportFinder
    {
        private readonly IEnumerable<OurAirport> airports;

        public OurAirportFinder(IJsonReader<IEnumerable<OurAirport>> reader, string airportFilepath)
            => airports = reader.FromFile(airportFilepath);

        public OurAirportFinder(IJsonReader<IEnumerable<OurAirport>> reader, IConfiguration config)
            : this(reader, config[ConfigKeys.AirportsFilepath]) { }

        public OurAirport SearchWithIcao(string icao) => airports.First(x => x.Ident == icao);

        public OurAirport SearchWithWaypoint(Waypoint waypoint)
        {
            var distanceMap = new Dictionary<OurAirport, double>();
            foreach (var airport in airports.Where(x => x.Latitude != 0 && x.Longitude != 0))
            {
                var distance = GpsCalculator.GetHaversineDistance(waypoint.Latitude, waypoint.Longitude, airport.Latitude, airport.Longitude);
                distanceMap.Add(airport, distance);
            }

            var nearest = distanceMap.OrderBy(x => x.Value).FirstOrDefault();
            return nearest.Key;
        }

        public OurRunway SearchRunwayWithWaypoint(Waypoint waypoint)
        {
            var airport = SearchWithWaypoint(waypoint);
            var distanceMap = new Dictionary<OurRunway, double>();
                foreach (var runway in airport.Runways)
                {
                    if (!runway.HeLatitude.HasValue || !runway.HeLongitude.HasValue || !runway.LeLatitude.HasValue || !runway.LeLongitude.HasValue)
                        continue;

                    var trackerror = GpsCalculator.GetCrossTrack(
                        runway.LeLatitude.Value, 
                        runway.LeLongitude.Value, 
                        runway.HeLatitude.Value, 
                        runway.HeLongitude.Value, 
                        waypoint.Latitude, 
                        waypoint.Longitude);

                    distanceMap.Add(runway, trackerror);
                }

            var ordered = distanceMap.OrderBy(x => x.Value);
            var nearest = ordered.FirstOrDefault().Key;
            return nearest;
        }
    }
}