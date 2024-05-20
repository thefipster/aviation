using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Geo;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Exceptions;

namespace TheFipster.Aviation.Modules.SimToolkitPro.Components
{
    public class SimToolkitProGeotagger
    {
        private readonly FlightFileScanner scanner;
        private readonly FlightMeta meta;
        private readonly JsonReader<Track> trackReader;

        public SimToolkitProGeotagger()
        {
            scanner = new FlightFileScanner();
            meta = new FlightMeta();
            trackReader = new JsonReader<Track>();
        }

        public GeoTagReport GeocodeScreenshots(FlightImport flight, string folder)
        {
            if (!flight.HasSimToolkitPro || !flight.HasTrack)
                throw new MissingConfigException("No SimtoolkitPro available");

            var departure = meta.GetDeparture(folder);
            var arrival = meta.GetArrival(folder);

            var logbook = flight.SimToolkitPro.Logbook;
            var track = trackReader.FromText(logbook.TrackedGeoJson);

            var report = new GeoTagReport(departure, arrival);
            var screenshots = scanner.GetFiles(folder, FileTypes.Screenshot);
            foreach (var screenshot in screenshots)
            {
                var info = new FileInfo(screenshot);
                var created = info.CreationTime;
                var modified = info.LastWriteTime;

                var dateTaken = DateTime.Compare(created, modified) < 0 ? created : modified;
                var unixDate = ((DateTimeOffset)dateTaken).ToUnixTimeSeconds();
                var tag = getTagFromTrack(logbook, track, screenshot, unixDate);

                if (tag != null)
                    report.GeoTags.Add(tag);
            }

            report.GeoTags = report.GeoTags.OrderBy(x => x.Timestamp).ToList();
            return report;
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
    }
}
