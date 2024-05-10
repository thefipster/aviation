using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.FlightApi.Models;

namespace TheFipster.Aviation.FlightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegsController : ControllerBase
    {
        private readonly ILogger<LegsController> _logger;
        private readonly IConfiguration _config;
        private readonly IFlightFinder _finder;

        public LegsController(
            ILogger<LegsController> logger, 
            IConfiguration config, 
            IFlightFinder finder)
        {
            _logger = logger;
            _config = config;
            _finder = finder;
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
                var departureFile = airportFiles.FirstOrDefault(x => x.Contains(leg.From.Trim()));
                var departure = new JsonReader<Domain.Datahub.Airport>().FromFile(departureFile);

                var arrivalFile = airportFiles.FirstOrDefault(x => x.Contains(leg.To.Trim()));
                var arrival = new JsonReader<Domain.Datahub.Airport>().FromFile(arrivalFile);

                var done = false;
                if (flights.Any(x => x.Contains(leg.From.Trim()) && x.Contains(leg.To.Trim())))
                    done = true;

                yield return new MapLeg(leg, departure, arrival, done);
            }
        }
    }
}
