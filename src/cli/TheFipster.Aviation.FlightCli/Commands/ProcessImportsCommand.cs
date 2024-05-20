using System.Threading.Tasks.Sources;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.BlackBox;
using TheFipster.Aviation.Modules.BlackBox.Components;
using TheFipster.Aviation.Modules.SimToolkitPro.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class ProcessImportsCommand : IFlightCommand<ProcessImportsOptions>
    {
        private readonly FlightFileScanner scanner;
        private readonly JsonReader<FlightImport> flightReader;
        private readonly JsonReader<BlackBoxFlight> blackboxReader;
        private readonly BlackboxOperations blackboxOperations;
        private readonly JsonReader<SimToolkitProFlight> stkpReader;
        private readonly SimToolkitProCompressor stkpCompressor;
        private readonly JsonReader<Track> stkpTrackReader;
        private readonly JsonWriter<FlightImport> flightWriter;
        private readonly BlackboxGeotagger blackboxGeotagger;
        private readonly SimToolkitProGeotagger stkpGeotagger;
        private readonly SimToolkitProScanner stkpScanner;

        public ProcessImportsCommand()
        {
            scanner = new FlightFileScanner();
            flightReader = new JsonReader<FlightImport>();
            blackboxReader = new JsonReader<BlackBoxFlight>();
            blackboxOperations = new BlackboxOperations();
            stkpReader = new JsonReader<SimToolkitProFlight>();
            stkpCompressor = new SimToolkitProCompressor();
            stkpTrackReader = new JsonReader<Track>();
            flightWriter = new JsonWriter<FlightImport>();
            blackboxGeotagger = new BlackboxGeotagger();
            stkpGeotagger = new SimToolkitProGeotagger();
            stkpScanner = new SimToolkitProScanner();
        }

        public void Run(ProcessImportsOptions options, IConfig config)
        {
            Console.WriteLine(ProcessImportsOptions.Welcome);
            Guard.EnsureConfig(config);

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
                    flight = GenerateDistance(flight);
                    flight = GenerateGeoTags(folder, flight);
                    flight = MergeStatsFromStkp(flight);

                    flightWriter.Write(flight, folder, true);
                }
                catch (FileNotFoundException)
                {
                    StdOut.Write(2, Emoji.YellowCircle, "skipping, there is no flight file");
                }
            }
        }

        private FlightImport MergeStatsFromStkp(FlightImport flight)
        {
            if (!flight.HasSimToolkitPro)
                return flight;

            LogbookStats stats = stkpScanner.Scan(flight.SimToolkitPro);

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
                    flight.Geotags = tags.GeoTags;

                return flight;
            }

            if (flight.HasSimToolkitPro && flight.HasTrack)
            {
                var tags = stkpGeotagger.GeocodeScreenshots(flight, folder);
                if (tags.GeoTags.Count > 0)
                    flight.Geotags = tags.GeoTags;

                return flight;
            }

            return flight;
        }

        public FlightImport GenerateDistance(FlightImport flight)
        {
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
                var geojson = stkpTrackReader.FromText(flight.SimToolkitPro.Logbook.TrackedGeoJson);
                var compressed = stkpCompressor.CompressTrack(geojson);
                var track = compressed.Features.First().Geometry.Coordinates.Select(x => new Coordinate(x));
                flight.Track = track;
                return flight;
            }

            StdOut.Write(2, Emoji.RedCircle, "no track found.");
            return flight;
        }
    }
}
