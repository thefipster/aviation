using Microsoft.AspNetCore.Mvc;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.FlightApi.Models;
using TheFipster.Aviation.Modules.Airports.Abstractions;

namespace TheFipster.Aviation.FlightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegsController : ControllerBase
    {
        private readonly ILogger<LegsController> _logger;
        private readonly IConfiguration _config;
        private readonly IFlightFinder _finder;
        private readonly IAirportFinder _airports;

        public LegsController(
            ILogger<LegsController> logger, 
            IConfiguration config, 
            IFlightFinder finder,
            IAirportFinder airports)
        {
            _logger = logger;
            _config = config;
            _finder = finder;
            _airports = airports;
        }

        [HttpGet(Name = "GetLegs")]
        public IEnumerable<MapLeg> GetLegs()
        {
            var legFile = _config["FlightPlanJson"];
            var airportFolder = _config["AirportsFolder"];
            var flightsFolder = _config["FlightsFolder"];

            var airportFiles = Directory.GetFiles(airportFolder);
            var flights = _finder.GetFlightFolders(flightsFolder);
            var legs = new JsonReader<List<Leg>>().FromFile(legFile);

            foreach (var leg in legs)
            {
                var departure = _airports.SearchWithIcao(leg.From.Trim());
                var arrival = _airports.SearchWithIcao(leg.To.Trim());

                var done = false;
                if (flights.Any(x => x.Contains(leg.From.Trim()) && x.Contains(leg.To.Trim())))
                    done = true;

                yield return new MapLeg(leg, departure, arrival, done);
            }
        }
    }
}
