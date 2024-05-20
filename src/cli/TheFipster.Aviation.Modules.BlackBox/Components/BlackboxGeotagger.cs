using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Geo;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.Domain.BlackBox;

namespace TheFipster.Aviation.Modules.BlackBox.Components
{
    public class BlackboxGeotagger
    {
        private readonly FlightMeta meta;
        private readonly FlightFileScanner scanner;

        public BlackboxGeotagger()
        {
            meta = new FlightMeta();
            scanner = new FlightFileScanner();
        }

        public GeoTagReport GeocodeScreenshots(FlightImport flight, string folder)
        {
            if (!flight.HasBlackbox)
                throw new MissingConfigException("No Blackbox available");

            var departure = meta.GetDeparture(folder);
            var arrival = meta.GetArrival(folder);

            var report = new GeoTagReport(departure, arrival);
            var screenshots = scanner.GetFiles(folder, FileTypes.Screenshot);
            foreach (var screenshot in screenshots)
            {
                var info = new FileInfo(screenshot);
                var created = info.CreationTime;
                var modified = info.LastWriteTime;

                var dateTaken = DateTime.Compare(created, modified) < 0 ? created : modified;
                var unixDate = ((DateTimeOffset)dateTaken).ToUnixTimeSeconds();

                var tag = getTagFromBlackbox(flight.Blackbox, screenshot, unixDate);

                if (tag != null)
                    report.GeoTags.Add(tag);
            }

            report.GeoTags = report.GeoTags.OrderBy(x => x.Timestamp).ToList();
            return report;
        }

        private static GeoTag getTagFromBlackbox(ICollection<Record> records, string screenshot, long unixDate)
        {
            var matches = records.Where(x => x.Timestamp > unixDate - 30 && x.Timestamp < unixDate + 30);
            if (!matches.Any())
                return null;

            var match = matches.OrderBy(x => Math.Abs(x.Timestamp - unixDate)).First();
            var file = Path.GetFileName(screenshot);
            file = file.Replace(Path.GetExtension(file), string.Empty);
            var tag = new GeoTag(file, match);
            return tag;
        }
    }
}
