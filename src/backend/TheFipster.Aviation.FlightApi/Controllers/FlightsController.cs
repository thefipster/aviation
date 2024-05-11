using Microsoft.AspNetCore.Mvc;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.Modules.FlightPlan.Abstractions;

namespace TheFipster.Aviation.FlightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly ILogger<FlightsController> _logger;
        private readonly IConfiguration _config;
        private readonly IFlightFinder _finder;
        private readonly IFlightFileScanner _scanner;
        private readonly IFlightMeta _meta;
        private readonly IOperationsPlan _plan;

        public FlightsController(
            ILogger<FlightsController> logger,
            IConfiguration config,
            IFlightFinder finder,
            IFlightFileScanner scanner,
            IFlightMeta meta,
            IOperationsPlan plan)
        {
            _logger = logger;
            _config = config;
            _finder = finder;
            _scanner = scanner;
            _meta = meta;
            _plan = plan;
        }

        [HttpGet(Name = "GetFlights")]
        public IEnumerable<Leg> GetFlights()
        {
            _logger.LogDebug("Called get flights.");
            var flightsFolder = _config["FlightsFolder"];
            var flights = _finder.GetFlightFolders(flightsFolder);

            foreach (var flight in flights)
            {
                var no = _meta.GetLeg(flight);
                var from = _meta.GetDeparture(flight);
                var to = _meta.GetArrival(flight);

                yield return new Leg(no, from, to);
            }
        }

        [HttpGet("stats", Name = "GetStats")]
        public IEnumerable<Stats> GetStats()
        {
            var flightsFolder = _config["FlightsFolder"];
            var folders = _finder.GetFlightFolders(flightsFolder);

            foreach (var folder in folders)
            {
                Stats? stats = null;
                try
                {
                    var file = _scanner.GetFile(folder, FileTypes.StatsJson);
                    stats = new JsonReader<Stats>().FromFile(file);
                }
                catch (FileNotFoundException) { /* skipping */ }

                if (stats != null)
                    yield return stats;
            }
        }

        [HttpGet("next", Name = "GetNextFlight")]
        public PlannedFlight GetNextFlight()
            => _plan.GetNextFlight();

        [HttpGet("last", Name = "GetLastFlight")]
        public Stats GetLastFlight()
            => _plan.GetLastFlight();

        [HttpGet("{departure}/{arrival}", Name = "GetFlight")]
        public SimBriefFlight GetFlight(string departure, string arrival)
        {
            var flightsFolder = _config["FlightsFolder"];
            var flightFolder = _finder.GetFlightFolder(flightsFolder, departure, arrival);
            var simbriefSearch = _scanner.GetFiles(flightFolder, FileTypes.SimbriefJson);
            if (simbriefSearch == null)
                throw new KeyNotFoundException($"Simbrief JSON file from {departure} to {arrival} was not found.");

            var simbrief = new JsonReader<SimBriefFlight>().FromFile(simbriefSearch.First());
            return simbrief;
        }

        [HttpGet("{departure}/{arrival}/ofp", Name = "GetOfp")]
        public ContentResult GetOfp(string departure, string arrival)
        {
            var flightsFolder = _config["FlightsFolder"];
            var flightFolder = _finder.GetFlightFolder(flightsFolder, departure, arrival);
            var ofpHtml = _scanner.GetFiles(flightFolder, FileTypes.OfpHtml);
            if (ofpHtml == null)
                throw new KeyNotFoundException($"OFP HTML file from {departure} to {arrival} was not found.");

            var html = System.IO.File.ReadAllText(ofpHtml.First());

            return base.Content(html, "text/html");
        }

        [HttpGet("{departure}/{arrival}/route", Name = "GetRoute")]
        public IEnumerable<Coordinate> GetRoute(string departure, string arrival)
        {
            var flightsFolder = _config["FlightsFolder"];
            var flightFolder = _finder.GetFlightFolder(flightsFolder, departure, arrival);
            var routeJson = _scanner.GetFiles(flightFolder, FileTypes.RouteJson);
            if (routeJson == null)
                throw new KeyNotFoundException($"Route JSON file from {departure} to {arrival} was not found.");

            var route = new JsonReader<Domain.Simbrief.Route>().FromFile(routeJson.First());
            return route.Coordinates;
        }

        [HttpGet("{departure}/{arrival}/track", Name = "GetTrack")]
        public IEnumerable<Coordinate> GetTrack(string departure, string arrival)
        {
            var flightsFolder = _config["FlightsFolder"];
            var flightFolder = _finder.GetFlightFolder(flightsFolder, departure, arrival);
            var files = _scanner.GetFiles(flightFolder, FileTypes.BlackBoxJson);
            if (files != null && files.Any())
            {
                var route = new JsonReader<BlackBoxFlight>().FromFile(files.First());
                return route.Records.Select(x => new Coordinate(x.LatitudeDecimals, x.LongitudeDecimals, x.GpsAltitudeMeters));
            }

            files = _scanner.GetFiles(flightFolder, FileTypes.TrackJson);
            if (files != null && files.Any())
            {
                var track = new JsonReader<Track>().FromFile(files.First());
                return track.Features.First().Geometry.Coordinates.Select(x => new Coordinate(x.First(), x.Last()));
            }

            throw new KeyNotFoundException($"BlackBox or Track file from {departure} to {arrival} was not found.");
        }

        [HttpGet("{departure}/{arrival}/waypoints", Name = "GetWaypoints")]
        public IEnumerable<Waypoint> GetWaypoint(string departure, string arrival)
        {
            var flightsFolder = _config["FlightsFolder"];
            var flightFolder = _finder.GetFlightFolder(flightsFolder, departure, arrival);
            var files = _scanner.GetFiles(flightFolder, FileTypes.WaypointsJson);
            if (files == null)
                throw new KeyNotFoundException($"Waypoint JSON file from {departure} to {arrival} was not found.");

            var result = new JsonReader<SimbriefWaypoints>().FromFile(files.First());
            return result.Waypoints;
        }

        [HttpGet("{departure}/{arrival}/landing", Name = "GetLanding")]
        public Landing GetLanding(string departure, string arrival)
        {
            var flightsFolder = _config["FlightsFolder"];
            var flightFolder = _finder.GetFlightFolder(flightsFolder, departure, arrival);
            var files = _scanner.GetFiles(flightFolder, FileTypes.LandingJson);
            if (files == null)
                throw new KeyNotFoundException($"Landing JSON file from {departure} to {arrival} was not found.");

            var result = new JsonReader<Landing>().FromFile(files.First());
            return result;
        }

        [HttpGet("{departure}/{arrival}/notams", Name = "GetNotams")]
        public IEnumerable<Notam> GetNotams(string departure, string arrival)
        {
            var flightsFolder = _config["FlightsFolder"];
            var flightFolder = _finder.GetFlightFolder(flightsFolder, departure, arrival);
            var files = _scanner.GetFiles(flightFolder, FileTypes.NotamJson);
            if (files == null)
                throw new KeyNotFoundException($"Notams JSON file from {departure} to {arrival} was not found.");

            var result = new JsonReader<NotamReport>().FromFile(files.First());
            return result.Notams;
        }

        [HttpGet("{departure}/{arrival}/logbook", Name = "GetLogbook")]
        public Logbook GetLogbook(string departure, string arrival)
        {
            var flightsFolder = _config["FlightsFolder"];
            var flightFolder = _finder.GetFlightFolder(flightsFolder, departure, arrival);
            var files = _scanner.GetFiles(flightFolder, FileTypes.LogbookJson);
            if (files == null)
                throw new KeyNotFoundException($"Logbook JSON file from {departure} to {arrival} was not found.");

            var result = new JsonReader<Logbook>().FromFile(files.First());
            return result;
        }
    }
}
