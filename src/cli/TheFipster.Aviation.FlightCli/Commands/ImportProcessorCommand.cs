using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Extensions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Geo;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Airports.Components;
using TheFipster.Aviation.Modules.BlackBox;
using TheFipster.Aviation.Modules.BlackBox.Components;
using TheFipster.Aviation.Modules.SimToolkitPro;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class ImportProcessorCommand : IFlightCommand<ImportProcessorOptions>
    {
        private readonly JsonReader<IEnumerable<OurAirport>> airportReader;
        private readonly FlightFileScanner scanner;
        private readonly JsonReader<FlightImport> flightReader;
        private readonly BlackboxOperations blackboxOperations;
        private readonly JsonWriter<FlightImport> flightWriter;
        private readonly BlackboxGeotagger blackboxGeotagger;
        private readonly StkpOps stkpOps;
        private OurAirportFinder airports;

        public ImportProcessorCommand()
        {
            airportReader = new JsonReader<IEnumerable<OurAirport>>();
            scanner = new FlightFileScanner();
            flightReader = new JsonReader<FlightImport>();
            blackboxOperations = new BlackboxOperations();
            flightWriter = new JsonWriter<FlightImport>();
            blackboxGeotagger = new BlackboxGeotagger();
            stkpOps = new StkpOps();
        }

        public void Run(ImportProcessorOptions options, IConfig config)
        {
            Console.WriteLine(ImportProcessorOptions.Welcome);
            Guard.EnsureConfig(config);

            airports = new OurAirportFinder(airportReader, config.OurAirportFile);

            var folders = options.GetFlightFolders(config.FlightsFolder);
            foreach (var folder in folders)
            {
                Console.WriteLine("\t" + folder);

                try
                {
                    var file = scanner.GetFile(folder, FileTypes.FlightJson);
                    var flight = flightReader.FromFile(file);

                    flight = GenerateTrack(flight);
                    flight = MergeStatsFromBlackbox(flight);
                    flight = GenerateGeoTags(folder, flight);
                    flight = MergeStatsFromStkp(flight);
                    flight = GetActualTakeoffAndLanding(flight);
                    flight = GenerateTrackDistance(flight);
                    flight = GenerateGcDistance(flight);
                    flight = GenerateRouteDistance(flight);

                    flightWriter.Write(flight, folder, true);
                }
                catch (FileNotFoundException)
                {
                    StdOut.Write(2, Emoji.YellowCircle, "skipping, there is no flight file");
                }
            }
        }

        private FlightImport GenerateRouteDistance(FlightImport flight)
        {
            if (!flight.HasSimbriefKml)
                return flight;

            var waypoints = new List<Waypoint>();
            var placemarks = flight.SimbriefKml.Kml.Document.Placemark.Where(x => x.Point != null);

            var departureIcao = flight.GetDeparture();
            var departure = airports.SearchWithIcao(departureIcao);

            var arrivalIcao = flight.GetArrival();
            var arrival = airports.SearchWithIcao(arrivalIcao);

            if (!placemarks.Any(x => x.Name.ToUpper() == departureIcao.ToUpper()))
                waypoints.Add(new Waypoint(departure));

            waypoints.AddRange(placemarks.Select(x => new Waypoint(x)));
                
            if (!waypoints.Any(x => x.Name.ToUpper() == arrivalIcao.ToUpper()))
                waypoints.Add(new Waypoint(arrival));

            var distance = 0d;
            for (int i = 1; i < waypoints.Count; i++)
            {
                var last = waypoints[i - 1];
                var cur = waypoints[i];

                distance += GpsCalculator.GetHaversineDistance(
                    last.Latitude,
                    last.Longitude,
                    cur.Latitude,
                    cur.Longitude);
            }

            flight.Stats.RouteDistance = (int)Math.Round(distance,0);

            return flight;
        }

        private FlightImport GenerateGcDistance(FlightImport flight)
        {
            if (flight.Stats == null)
                flight.Stats = new Stats();

            var departureIcao = flight.GetDeparture();
            var departure = airports.SearchWithIcao(departureIcao);

            var arrivalIcao = flight.GetArrival();
            var arrival = airports.SearchWithIcao(arrivalIcao);

            var gcDistance = GpsCalculator.GetHaversineDistance(
                    departure.Latitude,
                    departure.Longitude,
                    arrival.Latitude,
                    arrival.Longitude);

            flight.Stats.GreatCircleDistance = (int)Math.Round(gcDistance, 0);
            return flight;
        }

        public FlightImport GetActualTakeoffAndLanding(FlightImport flight)
        {
            if (!flight.HasEvents)
                return flight;

            var takeoff = flight.Events.FirstOrDefault(x => x.Name == FlightEvents.Takeoff);
            var actualTakeoff = getFlightTerminator(takeoff);
            if (actualTakeoff != null)
                flight.ActualDeparture = actualTakeoff;

            var landing = flight.Events.FirstOrDefault(x => x.Name == FlightEvents.Landing);
            var actualLanding = getFlightTerminator(landing);
            if (actualLanding != null)
                flight.ActualArrival = actualLanding;

            return flight;
        }

        private FlightTerminator? getFlightTerminator(Waypoint landing)
        {
            if (landing == null)
                return null;

            var runway = airports.SearchRunwayWithWaypoint(landing);
            OurAirport airport;
            if (runway == null)
                airport = airports.SearchWithWaypoint(landing);
            else
                airport = airports.SearchWithIcao(runway.AirportIdent);

            if (runway == null && airport?.Runways != null && airport.Runways.Count() == 1)
                runway = airport.Runways.First();

            if (airport != null)
                return new FlightTerminator(airport.Ident, runway);

            return null;
        }

        public FlightImport MergeStatsFromStkp(FlightImport flight)
        {
            if (!flight.HasSimToolkitPro)
                return flight;

            LogbookStats stats = stkpOps.ScanFlight(flight.SimToolkitPro);

            if (flight.Stats == null)
                flight.Stats = new Stats();

            flight.Stats.Merge(stats);

            return flight;
        }

        public FlightImport GenerateGeoTags(string folder, FlightImport flight)
        {
            if (flight.HasBlackbox)
            {
                var tags = blackboxGeotagger.GeocodeScreenshots(flight, folder);
                if (tags.GeoTags.Count > 0)
                {
                    if (flight.Geotags == null)
                        flight.Geotags = new List<GeoTag>();

                    foreach (var tag in tags.GeoTags)
                        if (!flight.Geotags.Any(x => x.Screenshot == tag.Screenshot))
                            flight.Geotags.Add(tag);

                    return flight;
                }
            }

            if (flight.HasSimToolkitPro && flight.HasTrack)
            {
                var tags = stkpOps.GeotagScreenshots(flight, folder);
                if (tags.GeoTags.Count > 0)
                {
                    if (flight.Geotags == null)
                        flight.Geotags = new List<GeoTag>();

                    foreach (var tag in tags.GeoTags)
                        if (!flight.Geotags.Any(x => x.Screenshot == tag.Screenshot))
                            flight.Geotags.Add(tag);

                    return flight;
                }
            }

            return flight;
        }

        public FlightImport GenerateTrackDistance(FlightImport flight)
        {
            if (!flight.HasTrack)
                return flight;

            var distance = 0d;
            for (int i = 1; i < flight.Track.Count(); i++)
            {
                var last = flight.Track.Skip(i - 1).First();
                var cur = flight.Track.Skip(i).First();
                distance += GpsCalculator.GetHaversineDistance(last.Latitude, last.Longitude, cur.Latitude, cur.Longitude);
            }

            if (flight.Stats == null)
                flight.Stats = new Stats();

            flight.Stats.TrackDistance = distance;

            return flight;
        }

        public FlightImport MergeStatsFromBlackbox(FlightImport flight)
        {
            if (!flight.HasBlackbox)
                return flight;

            var extrems = blackboxOperations.Scan(flight.Blackbox);
            flight.Events = extrems.Waypoints;

            if (flight.Stats == null)
                flight.Stats = new Stats();

            flight.Stats.Merge(extrems);

            return flight;
        }

        public FlightImport GenerateTrack(FlightImport flight)
        {
            if (flight.HasBlackbox)
            {
                var compressed = blackboxOperations.Compress(flight.Blackbox);
                var track = compressed.Select(x => new Coordinate(x));
                flight.Track = track;
                return flight;
            }

            if (flight.HasSimToolkitPro)
            {
                var geojson = stkpOps.ExtractTrack(flight.SimToolkitPro);
                var compressed = stkpOps.CompressTrack(geojson);
                var track = compressed.Features.First().Geometry.Coordinates.Select(x => new Coordinate(x));
                flight.Track = track;
                return flight;
            }

            StdOut.Write(2, Emoji.RedCircle, "no track found.");
            return flight;
        }
    }
}
