using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.SimToolkitPro;

namespace TheFipster.Aviation.FlightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        private readonly IConfiguration config;

        public AircraftController(IConfiguration config)
        {
            this.config = config;
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
    }
}
