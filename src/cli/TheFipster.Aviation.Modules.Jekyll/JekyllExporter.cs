using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Modules.Jekyll.Components;
using TheFipster.Aviation.Modules.Jekyll.Model;

namespace TheFipster.Aviation.Modules.Jekyll
{
    public class JekyllExporter
    {
        private string jekyllRoot;

        private string assetFolder;
        private string imageFolder;
        private string screenshotFolder;
        private string apiFolder;
        private string postFolder;

        private string airportFile;

        public JekyllExporter(string jekyllFolder, string airportFile)
        {
            this.airportFile = airportFile;

            jekyllRoot = jekyllFolder;
            postFolder = Path.Combine(jekyllRoot, "_posts");

            assetFolder = Path.Combine(jekyllRoot, "assets");
            apiFolder = Path.Combine(assetFolder, "api");
            imageFolder = Path.Combine(assetFolder, "images");
            screenshotFolder = Path.Combine(imageFolder, "screenshots");
        }

        public void ExportFlight(string folder)
        {
            var departure = new FlightMeta().GetDeparture(folder);
            var arrival = new FlightMeta().GetArrival(folder);

            var post = new FrontMatterPostExporter().GenerateFlightPost(folder, airportFile);
            var postPath = Path.Combine(postFolder, post.Name);
            new PlainWriter().Write(postPath, post.Frontmatter, true);

            var gpsData = new FlightGpsExporter().GenerateGpsApiData(folder);
            var gpsDataPath = Path.Combine(apiFolder, "flights", departure + arrival + "-gps.json");
            new JsonWriter<FlightGeo>().Write(gpsDataPath, gpsData, true);

            new ScreenshotExporter().Export(folder, screenshotFolder);

        }

        public void ExportCombined(string flightsFolder)
        {
            var track = new TrackExporter().GenerateCombinedTrack(flightsFolder);
            var trackFile = Path.Combine(apiFolder, "track-flown.json");
            new JsonWriter<List<Track>>().Write(trackFile, track, true);

            var plannedAirports = new AirportExporter().GetPlannedAirports(flightsFolder);
            var plannedAirportsFile = Path.Combine(apiFolder, "airports-planned.json");
            new JsonWriter<List<Location>>().Write(plannedAirportsFile, plannedAirports, true);

            var aircraft = new FrontmatterAircraftExporter().CreateFrontmatter(flightsFolder, airportFile);
            var aircraftFile = Path.Combine(jekyllRoot, "aircraft.html");
            new PlainWriter().Write(aircraftFile, aircraft, true);
        }
    }
}
