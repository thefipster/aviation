using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class GeoTagCommand
    {
        private HardcodedConfig config;

        public GeoTagCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(GeoTagOptions options)
        {
            Console.WriteLine("GeoTagging Screenshots.");
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];


            foreach (var folder in folders)
            {
                Console.Write($"\t {folder}");
                var screenshots = new FlightFileScanner().GetFiles(folder, FileTypes.Screenshot);

                BlackBoxFlight blackbox = null;
                Track track = null;
                Logbook logbook = null;

                var departure = new FlightMeta().GetDeparture(folder);
                var arrival = new FlightMeta().GetArrival(folder);
                GeoTagReport report = new GeoTagReport(departure, arrival);

                try
                {
                    var blackboxFile = new FlightFileScanner().GetFile(folder, FileTypes.BlackBoxJson);
                    blackbox = new JsonReader<BlackBoxFlight>().FromFile(blackboxFile);
                }
                catch (Exception)
                {
                    // lets hope there is a stkp export
                }

                try
                {
                    var logbookFile = new FlightFileScanner().GetFile(folder, FileTypes.LogbookJson);
                    logbook = new JsonReader<Logbook>().FromFile(logbookFile);

                    var trackFile = new FlightFileScanner().GetFile(folder, FileTypes.TrackJson);
                    track = new JsonReader<Track>().FromFile(trackFile);
                }
                catch (Exception)
                {
                    // uh oh, will catch afterwards
                }

                if (blackbox == null && (track == null || logbook == null))
                {
                    Console.WriteLine(" - skipping, no stkp or blackbox");
                    continue;
                }


                foreach (var screenshot in screenshots)
                {
                    var info = new FileInfo(screenshot);
                    var created = info.CreationTime;
                    var modified = info.LastWriteTime;

                    var dateTaken = DateTime.Compare(created, modified) < 0 ? created : modified;
                    var unixDate = ((DateTimeOffset)dateTaken).ToUnixTimeSeconds();

                    GeoTag tag = null;

                    if (blackbox != null)
                        tag = getTagFromBlackbox(blackbox, screenshot, unixDate);
                    else
                        tag = getTagFromTrack(logbook, track, screenshot, unixDate);

                    if (tag != null)
                        report.GeoTags.Add(tag);
                }

                report.GeoTags = report.GeoTags.OrderBy(x => x.Timestamp).ToList();

                if (!report.GeoTags.Any())
                {
                    Console.WriteLine(" - skipping, theres nothing I can do anymore.");
                    continue;
                }

                new JsonWriter<GeoTagReport>().Write(folder, report, FileTypes.GeoTagsJson, report.Departure, report.Arrival);
                Console.WriteLine();
            }
        }

        private GeoTag? getTagFromTrack(Logbook logbook, Track? track, string screenshot, long unixDate)
        {
            var coordinates = track.Features.First().Geometry.Coordinates;
            var start = int.Parse(logbook.ActualDep);
            var end = int.Parse(logbook.ActualArr);
            var duration = end - start;
            var step = (float)duration / coordinates.Count();

            var progress = unixDate - start;
            if (progress < 0 || progress > duration)
                return null;

            var index = (int)(progress / step);
            var match = coordinates[index];

            var file = Path.GetFileName(screenshot);
            file = file.Replace(Path.GetExtension(file), string.Empty);

            var tag = new GeoTag(file, unixDate, match);
            return tag;
        }

        private static GeoTag getTagFromBlackbox(BlackBoxFlight blackbox, string screenshot, long unixDate)
        {
            var records = blackbox.Records.Where(x => x.Timestamp > unixDate - 30 && x.Timestamp < unixDate + 30);
            if (!records.Any())
                return null;

            var match = records.OrderBy(x => Math.Abs(x.Timestamp - unixDate)).First();
            var file = Path.GetFileName(screenshot);
            file = file.Replace(Path.GetExtension(file), string.Empty);
            var tag = new GeoTag(file, match);
            return tag;
        }
    }
}
