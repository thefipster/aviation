using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Extensions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Modules.Airports.Components;
using TheFipster.Aviation.Modules.Jekyll.Components;
using TheFipster.Aviation.Modules.Jekyll.Model;

namespace TheFipster.Aviation.Modules.Jekyll
{
    public class JekyllExporter
    {
        private string jekyllRoot;

        private readonly string assetFolder;
        private readonly string dataFolder;
        private readonly string captureFolder;
        private readonly string apiFolder;
        private readonly string postFolder;

        private string airportFile;
        private readonly FlightFileScanner scanner;
        private readonly FlightMeta meta;
        private readonly JsonReader<FlightImport> flightReader;
        private readonly OurAirportFinder airports;

        public JekyllExporter(string jekyllFolder, string airportFile)
        {
            this.airportFile = airportFile;

            scanner = new FlightFileScanner();
            meta = new FlightMeta();
            flightReader = new JsonReader<FlightImport>();
            airports = new OurAirportFinder(new JsonReader<IEnumerable<OurAirport>>(), airportFile);

            jekyllRoot = jekyllFolder;
            postFolder = Path.Combine(jekyllRoot, "_posts");
            dataFolder = Path.Combine(jekyllRoot, "_data");

            assetFolder = Path.Combine(jekyllRoot, "assets");
            apiFolder = Path.Combine(assetFolder, "api");
            captureFolder = Path.Combine(assetFolder, "caps");
        }

        public void ExportFlight(string folder)
        {
            var flightFile = scanner.GetFile(folder, FileTypes.FlightJson);
            var flight = flightReader.FromFile(flightFile);

            var departure = meta.GetDeparture(folder);
            var arrival = meta.GetArrival(folder);
            var leg = meta.GetLeg(folder);

            var post = new FrontMatterPostExporter().GenerateFlightPost(flight, airports);
            var postPath = Path.Combine(postFolder, post.Name);
            new PlainWriter().Write(postPath, post.Frontmatter, true);

            var gpsData = new FlightGpsExporter().GenerateGpsApiData(flight);
            var gpsDataPath = Path.Combine(apiFolder, "flights", leg + "-" + departure + "-" + arrival + "-gps.json");
            new JsonWriter<FlightGeo>().Write(gpsDataPath, gpsData, true);

            new ScreenshotExporter().GenerateImages(folder, captureFolder);
        }

        public void ExportCombined(string flightsFolder)
        {
            var track = new TrackExporter().GenerateCombinedTrack(flightsFolder);
            var trackFile = Path.Combine(apiFolder, "track-flown.json");
            new JsonWriter<List<Track>>().Write(trackFile, track, true);

            var plannedAirports = new AirportExporter().GetPlannedAirports(flightsFolder, airports);
            var plannedAirportsFile = Path.Combine(apiFolder, "airports-planned.json");
            new JsonWriter<List<Location>>().Write(plannedAirportsFile, plannedAirports, true);

            var aircraft = new AircraftExporter().FromCombinedFlights(flightsFolder, airportFile);
            var yaml = new YamlWriter().ToYaml(aircraft);
            var aircraftFile = Path.Combine(dataFolder, "aircraft.yml");
            new PlainWriter().Write(aircraftFile, yaml, true);

            var parking = new decimal[] { 
                aircraft.Position.Latitude.RoundToSignificantDigits(5), 
                aircraft.Position.Longitude.RoundToSignificantDigits(5)};
            var parkingFile = Path.Combine(apiFolder, "park-position.json");
            new JsonWriter<IEnumerable<int>>().Write(parkingFile, parking, true);

            var fuelChart = new ChartExporter().ExportFuelChart(flightsFolder);
            var fuelChartFile = Path.Combine(apiFolder, "line-milage.json");
            new JsonWriter<IEnumerable<int>>().Write(fuelChartFile, fuelChart, true);

            var speedChart = new ChartExporter().ExportSpeedChart(flightsFolder);
            var speedChartFile = Path.Combine(apiFolder, "line-groundspeed.json");
            new JsonWriter<IEnumerable<int>>().Write(speedChartFile, speedChart, true);
        }
    }
}
