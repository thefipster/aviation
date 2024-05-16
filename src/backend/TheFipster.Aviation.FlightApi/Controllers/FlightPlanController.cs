using Microsoft.AspNetCore.Mvc;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.FlightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private readonly IFlightPlanReader reader;
        private readonly IFlightPlanWriter writer;

        public FlightPlanController(IFlightPlanReader reader, IFlightPlanWriter writer)
        {
            this.reader = reader;
            this.writer = writer;
        }

        [HttpGet]
        public IEnumerable<Leg> GetPlan()
            => reader.GetFlightPlan();

        [HttpPost]
        public void PostPlan([FromBody] IEnumerable<Leg> plan)
            => writer.SaveFlightPlan(plan);
    }
}
