using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Extensions;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Modules.Jekyll.Model;
using System.Collections.Concurrent;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    internal class TrackExporter
    {
        private readonly FlightFileScanner scanner;
        private readonly JsonReader<FlightImport> flightReader;
        private readonly FlightMeta meta;

        public TrackExporter()
        {
            scanner = new FlightFileScanner();
            flightReader = new JsonReader<FlightImport>();
            meta = new FlightMeta();
        }

        public List<Track> GenerateCombinedTrack(string flightsFolder)
        {
            var combinedTrack = new ConcurrentBag<Track>();

            var folders = Directory.GetDirectories(flightsFolder);
            Parallel.ForEach(folders, folder =>
            {
                if (folder.Contains("42"))
                    return;

                var flightFile = scanner.GetFile(folder, FileTypes.FlightJson);
                var flight = flightReader.FromFile(flightFile);
                var flightNo = meta.GetLeg(folder);
                var track = flight.Track.Select(x => new List<decimal>() { x.Latitude.RoundToSignificantDigits(5), x.Longitude.RoundToSignificantDigits(5) }).ToList();
                var name = flight.Departure + " - " + flight.Arrival;
                var uri = MetaInformation.GeneratePostUrl(flight.FlightNumber, flight.Departure, flight.Arrival);

                combinedTrack.Add(new Track(flightNo, name, uri, track));
            });
            return combinedTrack.OrderBy(x => x.FlightNo).ToList();
        }
    }
}
