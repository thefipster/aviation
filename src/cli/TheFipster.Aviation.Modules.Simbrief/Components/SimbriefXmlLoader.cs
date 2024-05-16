using System.Globalization;
using System.Xml.Linq;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Modules.Simbrief.Components
{
    public class SimbriefXmlLoader
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

            appendWeather(doc, flight);


            return flight;
        }

        public NotamReport ReadNotams(string filepath)
        {
            var xml = File.ReadAllText(filepath);
            var doc = XElement.Parse(xml);
            var notams = extractNotams(doc).ToList();
            var departure = extractAirport(doc, "origin");
            var arrival = extractAirport(doc, "destination");

            var report = new NotamReport(departure.Icao, arrival.Icao, notams);
            return report;
        }

        public string ReadOfp(string filepath)
        {
            var xml = File.ReadAllText(filepath);
            var doc = XElement.Parse(xml);
            var text = doc.Descendants("plan_html").FirstOrDefault();

            return text.Value;
        }

        private IEnumerable<Notam> extractNotams(XElement doc)
        {
            var notamsNode = doc.Descendants("notams");
            if (notamsNode == null)
                yield break;

            var notamRec = notamsNode.Descendants("notamdrec");
            foreach (var rec in notamRec)
            {
                var notam = new Notam();

                notam.Account = rec.Descendants("account_id").FirstOrDefault()?.Value;
                notam.Icao = rec.Descendants("icao_id").FirstOrDefault()?.Value;
                notam.Id = rec.Descendants("notam_id").FirstOrDefault()?.Value;
                notam.Name = rec.Descendants("icao_name").FirstOrDefault()?.Value;
                notam.Report = rec.Descendants("notam_report").FirstOrDefault()?.Value;
                notam.Source = rec.Descendants("source_id").FirstOrDefault()?.Value;
                notam.Text = rec.Descendants("notam_text").FirstOrDefault()?.Value;

                yield return notam;
            }
        }

        private void appendWeather(XElement doc, SimBriefFlight flight)
        {
            var weatherNode = doc.Descendants("weather").FirstOrDefault();
            if (weatherNode == null)
                return;

            if (flight.Departure != null)
            {
                var origin_metar = weatherNode.Descendants("orig_metar").FirstOrDefault()?.Value;
                var origin_taf = weatherNode.Descendants("orig_taf").FirstOrDefault()?.Value;
                flight.Departure.Metar = origin_metar;
                flight.Departure.Taf = origin_taf;
            }

            if (flight.Arrival != null)
            {
                var dest_metar = weatherNode.Descendants("dest_metar").FirstOrDefault()?.Value;
                var dest_taf = weatherNode.Descendants("dest_taf").FirstOrDefault()?.Value;
                flight.Arrival.Metar = dest_metar;
                flight.Arrival.Taf = dest_taf;
            }

            if (flight.Alternate != null)
            {
                var altn_metar = weatherNode.Descendants("altn_metar").FirstOrDefault()?.Value;
                var altn_taf = weatherNode.Descendants("altn_taf").FirstOrDefault()?.Value;
                flight.Alternate.Metar = altn_metar;
                flight.Alternate.Taf = altn_taf;
            }
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

        private void appendWaypoints(XElement doc, SimBriefFlight flight)
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
                    Latitude = double.Parse(latitude, CultureInfo.InvariantCulture),
                    Longitude = double.Parse(longitude, CultureInfo.InvariantCulture),
                    Name = ident
                };

                flight.Waypoints.Add(waypoint);
            }
        }

        private void appendParameters(XElement doc, SimBriefFlight flight)
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

        private void appendGeneral(XElement doc, SimBriefFlight flight)
        {
            var general = doc.Descendants("general").FirstOrDefault();
            if (general != null)
            {
                var airline = general.Descendants("icao_airline").FirstOrDefault()?.Value;
                var flightNumber = general.Descendants("flight_number").FirstOrDefault()?.Value;
                var costIndex = general.Descendants("costindex").FirstOrDefault()?.Value;
                var plannedAltitude = general.Descendants("initial_altitude").FirstOrDefault()?.Value;
                var averageWindComponent = general.Descendants("avg_wind_comp").FirstOrDefault()?.Value;
                var averageWindDirection = general.Descendants("avg_wind_dir").FirstOrDefault()?.Value;
                var averageWindSpeed = general.Descendants("avg_wind_spd").FirstOrDefault()?.Value;
                var greatCircleDistance = general.Descendants("gc_distance").FirstOrDefault()?.Value;
                var routeDistance = general.Descendants("route_distance").FirstOrDefault()?.Value;
                var airDistance = general.Descendants("air_distance").FirstOrDefault()?.Value;
                var fuelburn = general.Descendants("total_burn").FirstOrDefault()?.Value;
                var passengers = general.Descendants("passengers").FirstOrDefault()?.Value;
                var route = general.Descendants("route_navigraph").FirstOrDefault()?.Value;
                var plannedTas = general.Descendants("cruise_tas").FirstOrDefault()?.Value;
                var plannedMach = general.Descendants("cruise_mach").FirstOrDefault()?.Value;

                flight.Airline = airline;
                flight.FlightNumber = flightNumber;
                flight.CostIndex = int.Parse(costIndex);
                flight.Altitude = int.Parse(plannedAltitude);
                flight.WindComponent = int.Parse(averageWindComponent);
                flight.WindDirection = int.Parse(averageWindDirection);
                flight.WindSpeed = int.Parse(averageWindSpeed);
                flight.GreatCircleDistance = int.Parse(greatCircleDistance);
                flight.RouteDistance = int.Parse(routeDistance);
                flight.AirDistance = int.Parse(airDistance);
                flight.FuelBurn = int.Parse(fuelburn);
                flight.Passengers = int.Parse(passengers);
                flight.PlannedTas = int.Parse(plannedTas);
                flight.PlannedMach = float.Parse(plannedMach, CultureInfo.InvariantCulture);
                flight.Route = route;
            }
        }
    }
}
