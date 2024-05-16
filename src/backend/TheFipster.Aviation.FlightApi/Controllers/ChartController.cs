using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Modules.FlightPlan.Abstractions;

namespace TheFipster.Aviation.FlightApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IFlightFinder _finder;
        private readonly IFlightFileScanner _scanner;

        public ImageController(
            IConfiguration config,
            IFlightFinder finder,
            IFlightFileScanner scanner)
        {
            _config = config;
            _finder = finder;
            _scanner = scanner;
        }

        [HttpGet("{departure}/{arrival}/{screenshot}", Name = "GetFlightScreenshot")]
        public FileResult GetScreenshot(string departure, string arrival, string screenshot)
        {
            var flightsFolder = _config["FlightsFolder"];
            var flightFolder = _finder.GetFlightFolder(flightsFolder, departure, arrival);
            var files = _scanner.GetFiles(flightFolder, FileTypes.Screenshot);
            var filename = WebUtility.UrlDecode(screenshot) + ".png";
            var file = files.FirstOrDefault(x => Path.GetFileName(x) == filename);
            var bin = System.IO.File.ReadAllBytes(file);
            return File(bin, MediaTypeNames.Image.Png);
        }

        [HttpGet("thumbnail/{departure}/{arrival}/{screenshot}", Name = "GetFlightThumbnail")]
        public FileResult GetThumbnail(string departure, string arrival, string screenshot)
        {
            var flightsFolder = _config["FlightsFolder"];
            var flightFolder = _finder.GetFlightFolder(flightsFolder, departure, arrival);
            var files = _scanner.GetFiles(flightFolder, FileTypes.Preview);
            var filename = WebUtility.UrlDecode(screenshot) + ".jpg";
            var file = files.FirstOrDefault(x => Path.GetFileName(x) == filename);
            var bin = System.IO.File.ReadAllBytes(file);
            return File(bin, MediaTypeNames.Image.Jpeg);
        }
    }
}
