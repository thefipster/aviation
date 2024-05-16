using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Modules.Jekyll.Model;
using TheFipster.Aviation.Modules.Airports.Components;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Domain.Extensions;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    internal class FrontmatterAircraftExporter
    {
        internal string CreateFrontmatter(string flightsFolder, string airportFile)
        {
            var folders = Directory.GetDirectories(flightsFolder);

            var aircraft = new Aircraft();
            aircraft.Flights = folders.Count();

            foreach (var folder in folders)
            {
                try
                {
                    var blackboxFile = new FlightFileScanner().GetFile(folder, FileTypes.BlackBoxJson);
                    var blackbox = new JsonReader<BlackBoxFlight>().FromFile(blackboxFile);
                    processBlackBox(aircraft, blackbox);
                }
                catch (Exception)
                {
                    // no blackbox, lets move on
                }

                var gpsFile = new FlightFileScanner().GetFile(folder, FileTypes.GpsJson);
                var gps = new JsonReader<GpsReport>().FromFile(gpsFile);
                processGpsCoordinates(aircraft, gps.Coordinates);

                var statsFile = new FlightFileScanner().GetFile(folder, FileTypes.StatsJson);
                var stats = new JsonReader<Stats>().FromFile(statsFile);
                processStats(aircraft, stats);
            }

            processAirport(airportFile, aircraft);

            var frontmatter = new YamlWriter().ConvertToFrontmatter(aircraft);
            return frontmatter;
        }

        private static void processAirport(string airportFile, Aircraft aircraft)
        {
            var airport = new OurAirportFinder(new JsonReader<IEnumerable<OurAirport>>(), airportFile).SearchWithIcao(aircraft.Icao);

            aircraft.Parking = new Location("Parking spot", aircraft.Latitude, aircraft.Longitude);
            aircraft.Airport = airport.Name;
            aircraft.Country = airport.Country.Name;
            aircraft.Region = airport.Region.Name;
            aircraft.Continent = Continents.Dictionary[airport.ContinentCode];
        }

        private void processStats(Aircraft aircraft, Stats stats)
        {
            aircraft.Icao = stats.Arrival;
            aircraft.FuelConsumedKg += stats.FuelUsed;
            aircraft.SecondsFlown += stats.FlightTime;
            aircraft.Passengers += stats.Passengers;
        }

        private void processGpsCoordinates(Aircraft aircraft, ICollection<Coordinate> coordinates)
        {
            var coords = coordinates.ToList();
            for (int i = 1; i < coordinates.Count; i++)
            {
                var last = coords[i - 1];
                var cur = coords[i];
                var step = GpsCalculator.GetHaversineDistance(last.Latitude, last.Longitude, cur.Latitude, cur.Longitude);

                aircraft.DistanceFlownKm += step;
                aircraft.Latitude = cur.Latitude.RoundToSignificantDigits(6);
                aircraft.Longitude = cur.Longitude.RoundToSignificantDigits(6);
            }
        }

        private static void processBlackBox(Aircraft aircraft, BlackBoxFlight blackbox)
        {
            bool strike;
            foreach (var record in blackbox.Records)
            {
                if (record.GroundSpeedMps > aircraft.HighestSpeedMps)
                    aircraft.HighestSpeedMps = record.GroundSpeedMps;

                if (record.GpsAltitudeMeters > aircraft.HighestAltitudeM)
                    aircraft.HighestAltitudeM = record.GpsAltitudeMeters;

                if (record.AltimeterFeet < 10000 && record.IndicatedAirSpeedKnots > 250)
                    strike = true;
            }

            aircraft.OverspeedBelow10000 += 1;
        }
    }
}
