using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.Domain.Geo;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Airports.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Takes the blackbox or track file, geotag file, simbrief file and combines all geo information outputting the gps file.
    /// </summary>
    internal class CombineGpsInformationCommand : IFlightCommand<CombineGpsInformationOptions>
    {
        private IConfig config;

        public void Run(CombineGpsInformationOptions options, IConfig config)
        {
            Console.WriteLine("Generate gps file from blackbox or track (blackbox wins) and waypoints.");

            this.config = config;
            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            foreach (var folder in folders)
            {
                Console.Write($"\t {folder}");

                var departure = new FlightMeta().GetDeparture(folder);
                var arrival = new FlightMeta().GetArrival(folder);

                List<Coordinate> coordinates;
                try
                {
                    coordinates = getCoordinates(folder);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($" - skipping, no blackbox or trackfile.");
                    continue;
                }

                List<Waypoint> waypoints;
                try
                {
                    waypoints = getWaypoints(folder);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine($" - skipping, no waypoints.");
                    waypoints = new List<Waypoint>();
                }

                List<GeoTag> geoTags;
                try
                {
                    geoTags = getGeoTags(folder);
                }
                catch (FileNotFoundException)
                {
                    // no screenshots no geo tags, we will survive
                    geoTags = new List<GeoTag>();
                }

                List<Waypoint> blackBoxEvents;
                try
                {
                    blackBoxEvents = getBlackboxEvents(folder);
                }
                catch (FileNotFoundException)
                {
                    // no blackbox no events, we will survive just barely
                    blackBoxEvents = new List<Waypoint>();
                }

                var gpsReport = new GpsReport(departure, arrival, coordinates, waypoints, geoTags, blackBoxEvents);
                new JsonWriter<GpsReport>().Write(folder, gpsReport, FileTypes.GpsJson, gpsReport.Departure, gpsReport.Arrival, true);
                Console.WriteLine($" - {coordinates.Count} coordinates & {waypoints.Count} waypoints.");
            }
        }

        private List<Waypoint> getBlackboxEvents(string folder)
        {
            var blackboxStatsFile = new FlightFileScanner().GetFile(folder, FileTypes.BlackBoxStatsJson);
            var stats = new JsonReader<BlackBoxStats>().FromFile(blackboxStatsFile);
            return stats.Waypoints.ToList();
        }

        private List<GeoTag> getGeoTags(string folder)
        {
            var geotagsFile = new FlightFileScanner().GetFile(folder, FileTypes.GeoTagsJson);
            var geotags = new JsonReader<GeoTagReport>().FromFile(geotagsFile);
            return geotags.GeoTags.ToList();
        }

        private List<Waypoint> getWaypoints(string folder)
        {
            var simbriefFile = new FlightFileScanner().GetFile(folder, FileTypes.SimbriefJson);
            var simbrief = new JsonReader<SimBriefFlight>().FromFile(simbriefFile);

            var reader = new JsonReader<IEnumerable<Domain.Datahub.Airport>>();
            var airports = new AirportFinder(reader, config.AirportFile);
            var departure = airports.SearchWithIcao(simbrief.Departure.Icao);
            var alternate = airports.SearchWithIcao(simbrief.Alternate.Icao);

            var departureWaypoint = new Waypoint(0, "DEP", departure);
            var result = new List<Waypoint>();
            result.Add(departureWaypoint);

            var waypointFile = new FlightFileScanner().GetFile(folder, FileTypes.WaypointsJson);
            var waypoints = new JsonReader<SimbriefWaypoints>().FromFile(waypointFile);

            result.AddRange(waypoints.Waypoints);

            var alternateWaypoint = new Waypoint(result.Count, "ALT", alternate);
            result.Add(alternateWaypoint);

            return result;
        }

        private List<Coordinate> getCoordinates(string folder)
        {
            BlackBoxFlight blackbox = null;
            Track track = null;

            try
            {
                var blackboxfile = new FlightFileScanner().GetFile(folder, FileTypes.BlackBoxCompressedJson);
                blackbox = new JsonReader<BlackBoxFlight>().FromFile(blackboxfile);
            }
            catch (Exception)
            {
                // let's hope there is a track file
            }

            if (blackbox == null)
            {
                try
                {
                    var trackFile = new FlightFileScanner().GetFile(folder, FileTypes.TrackCompressedJson);
                    track = new JsonReader<Track>().FromFile(trackFile);
                }
                catch (Exception)
                {
                    throw new FileNotFoundException("Couldn't find blackbox or track file.");
                }
            }

            var coordinates = new List<Coordinate>();

            if (blackbox != null)
            {
                foreach (var record in blackbox.Records)
                {
                    var coordinate = new Coordinate(record);
                    coordinates.Add(coordinate);
                }
            }
            else
            {
                foreach (var point in track.Features.First().Geometry.Coordinates)
                {
                    var coordinate = new Coordinate(point);
                    coordinates.Add(coordinate);
                }
            }

            return coordinates;
        }
    }
}