using Microsoft.AspNetCore.Mvc;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.Modules.Airports.Abstractions;

namespace TheFipster.Aviation.FlightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private IAirportFinder airports;

        public AirportController(IAirportFinder airports)
            => this.airports = airports;

        [HttpGet("{icao}", Name = "GetAirport")]
        public Airport GetAirport(string icao)
            => airports.SearchWithIcao(icao.ToUpper());
    }
}
