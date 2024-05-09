using Microsoft.AspNetCore.Mvc;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.FlightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegsController : ControllerBase
    {
        private readonly ILogger<LegsController> _logger;
        private readonly IConfiguration _config;
        private readonly IFileSystemFinder _finder;

        public LegsController(
            ILogger<LegsController> logger, 
            IConfiguration config, 
            IFileSystemFinder finder)
        {
            _logger = logger;
            _config = config;
            _finder = finder;
        }

        [HttpGet(Name = "GetLegs")]
        public IEnumerable<Leg> GetLegs()
        {
            var legFile = _config["FlightPlanJson"];
            var legs = new JsonReader<List<Leg>>().FromFile(legFile);
            return legs;
        }
    }
}
