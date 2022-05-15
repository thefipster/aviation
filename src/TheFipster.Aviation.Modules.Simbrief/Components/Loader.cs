using System.Globalization;
using System.Xml.Linq;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Modules.Simbrief.Components
{
    public class Loader
    {
        public SimBriefFlight Read(string filepath)
        {
            var xml = File.ReadAllText(filepath);
            var doc = XElement.Parse(xml);
            var flight = new SimBriefFlight();

            appendParameters(doc, flight);
            appendGeneral(doc, flight);
            appendWaypoints(doc, flight);

            flight.Departure = extractAirport(doc, "origin");
            flight.Arrival = extractAirport(doc, "destination");
            flight.Alternate = extractAirport(doc, "alternate");

            return flight;
        }

        private Airport extractAirport(XElement doc, string node)
        {
            var airport = new Airport();

            var airportNode = doc.Descendants(node).FirstOrDefault();
            if (airportNode != null)
            {
                var icao = airportNode.Descendants("icao_code").FirstOrDefault()?.Value;
                var elevation = airportNode.Descendants("elevation").FirstOrDefault()?.Value;
                var latitude = airportNode.Descendants("pos_lat").FirstOrDefault()?.Value;
                var longitude = airportNode.Descendants("pos_long").FirstOrDefault()?.Value;
                var name = airportNode.Descendants("name").FirstOrDefault()?.Value;
                var runway = airportNode.Descendants("plan_rwy").FirstOrDefault()?.Value;
                var transitionAltitude = airportNode.Descendants("trans_alt").FirstOrDefault()?.Value;
                var transitionLevel = airportNode.Descendants("trans_level").FirstOrDefault()?.Value;

                airport.Icao = icao;
                airport.Elevation = int.Parse(elevation);
                airport.Latitude = double.Parse(latitude, CultureInfo.InvariantCulture);
                airport.Longitude = double.Parse(longitude, CultureInfo.InvariantCulture);
                airport.Name = name;
                airport.Runway = runway;
                airport.TransitionAltitude = int.Parse(transitionAltitude);
                airport.TransitionLevel = int.Parse(transitionLevel);
            }

            return airport;
        }

        private static void appendWaypoints(XElement doc, SimBriefFlight flight)
        {
            var index = 0;
            var fixes = doc.Descendants("fix");
            foreach (var fix in fixes)
            {
                index++;
                var ident = fix.Descendants("ident").FirstOrDefault()?.Value;
                var latitude = fix.Descendants("pos_lat").FirstOrDefault()?.Value;
                var longitude = fix.Descendants("pos_long").FirstOrDefault()?.Value;
                var airway = fix.Descendants("via_airway").FirstOrDefault()?.Value;

                var waypoint = new Waypoint
                {
                    Index = index,
                    Airway = airway,
                    Latitude = double.Parse(latitude),
                    Longitude = double.Parse(longitude),
                    Name = ident
                };

                flight.Waypoints.Add(waypoint);
            }
        }

        private static void appendParameters(XElement doc, SimBriefFlight flight)
        {
            var parameters = doc.Descendants("params").FirstOrDefault();
            if (parameters != null)
            {
                var timestamp = parameters.Descendants("time_generated").FirstOrDefault()?.Value;
                var airac = parameters.Descendants("airac").FirstOrDefault()?.Value;

                flight.DispatchDate = long.Parse(timestamp);
                flight.AiracCycle = airac;
            }
        }

        private static void appendGeneral(XElement doc, SimBriefFlight flight)
        {
            var general = doc.Descendants("general").FirstOrDefault();
            if (general != null)
            {
                var airline = general.Descendants("icao_airline").FirstOrDefault()?.Value;
                var flightNumber = general.Descendants("flight_number").FirstOrDefault()?.Value;
                var costIndex = general.Descendants("costindex").FirstOrDefault()?.Value;
                var plannedAltitude = general.Descendants("initial_altitude").FirstOrDefault()?.Value;
                var averageWindComponent = general.Descendants("avg_wind_comp").FirstOrDefault()?.Value;
                var greatCircleDistance = general.Descendants("gc_distance").FirstOrDefault()?.Value;
                var routeDistance = general.Descendants("route_distance").FirstOrDefault()?.Value;
                var airDistance = general.Descendants("air_distance").FirstOrDefault()?.Value;
                var passengers = general.Descendants("passengers").FirstOrDefault()?.Value;
                var route = general.Descendants("route_navigraph").FirstOrDefault()?.Value;

                flight.Airline = airline;
                flight.FlightNumber = flightNumber;
                flight.CostIndex = int.Parse(costIndex);
                flight.Altitude = int.Parse(plannedAltitude);
                flight.WindComponent = int.Parse(averageWindComponent);
                flight.GreatCircleDistance = int.Parse(greatCircleDistance);
                flight.RouteDistance = int.Parse(routeDistance);
                flight.AirDistance = int.Parse(airDistance);
                flight.Passengers = int.Parse(passengers);
                flight.Route = route;
            }
        }
    }
}
