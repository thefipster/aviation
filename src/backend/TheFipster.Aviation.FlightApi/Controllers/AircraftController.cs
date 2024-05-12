using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Reflection;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.FlightApi.Models;
using TheFipster.Aviation.Modules.Airports.Abstractions;
using TheFipster.Aviation.Modules.FlightPlan.Abstractions;

namespace TheFipster.Aviation.FlightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IFlightFinder finder;
        private readonly ILogger<AircraftController> logger;
        private readonly IFlightFileScanner scanner;
        private readonly IAirportFinder airports;

        public AircraftController(
            ILogger<AircraftController> logger,
            IConfiguration config,
            IFlightFinder finder,
            IFlightFileScanner scanner,
            IAirportFinder airports)
        {
            this.config = config;
            this.finder = finder;
            this.logger = logger;
            this.scanner = scanner;
            this.airports = airports;
        }

        [HttpGet(Name = "GetAircraft")]
        public Fleet Get()
        {
            var aircraftFile = config["AircraftJson"];
            var aircraft = new JsonReader<Fleet>().FromFile(aircraftFile);
            return aircraft;
        }

        [HttpGet("image", Name = "GetAircraftImage")]
        public FileContentResult GetImage()
        {
            var aircraftFile = config["AircraftImage"];
            var bin = System.IO.File.ReadAllBytes(aircraftFile);
            return File(bin, MediaTypeNames.Image.Jpeg);
        }

        [HttpGet("position", Name = "GetAircraftPosition")]
        public Point GetPosition()
        {
            var aircraftFile = config["AircraftJson"];
            var aircraft = new JsonReader<Fleet>().FromFile(aircraftFile);

            var flightsFolder = config["FlightsFolder"];
            var flights = finder.GetFlightFolders(flightsFolder);
            var flight = flights.Last();
            var blackboxFile = scanner.GetFile(flight, Domain.Enums.FileTypes.BlackBoxTrimmedJson);
            var blackbox = new JsonReader<BlackBoxFlight>().FromFile(blackboxFile);
            var lastRecord = blackbox.Records.Last();
            var airport = airports.SearchWithIcao(blackbox.Destination);

            return new Point(airport.Ident, airport.Name, lastRecord.LatitudeDecimals, lastRecord.LongitudeDecimals);
        }
    }
}
