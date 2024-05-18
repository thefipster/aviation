using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class CompressTrackCommand : IFlightCommand<CompressTrackOptions>
    {
        public void Run(CompressTrackOptions options, IConfig config)
        {
            Console.WriteLine("Optimizing STKP track file.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            Track track;
            foreach (var folder in folders)
            {
                Console.Write($"\t {folder}");
                try
                {
                    var file = new FlightFileScanner().GetFile(folder, FileTypes.TrackJson);
                    track = new JsonReader<Track>().FromFile(file);
                }
                catch (Exception)
                {
                    Console.WriteLine(" - skipping, no track file");
                    continue;
                }

                var coordinates = track.Features.First().Geometry.Coordinates.ToList();
                for (int i = coordinates.Count - 2; i >= 1; i--)
                {
                    var lat1 = coordinates[i - 1][1];
                    var lon1 = coordinates[i - 1][0];
                    var lat2 = coordinates[i][1];
                    var lon2 = coordinates[i][0];
                    var lat3 = coordinates[i + 1][1];
                    var lon3 = coordinates[i + 1][0];

                    var bearing1 = GpsCalculator.GetBearing(lat1, lon1, lat2, lon2);
                    var bearing2 = GpsCalculator.GetBearing(lat2, lon2, lat3, lon3);
                    var angle = Math.Abs(bearing1 - bearing2);

                    var distance = GpsCalculator.GetHaversineDistance(lat2, lon2, lat3, lon3);
                    if (angle < 10 && distance < 10)
                        coordinates.RemoveAt(i);
                }

                var compressedTrack = new Track
                {
                    Arrival = track.Arrival,
                    Departure = track.Departure,
                    Type = track.Type,
                    Features = new List<Feature>
                    {
                        new Feature
                        {
                            Type = track.Features.First().Type,
                            Geometry = new Geometry
                            {
                                Type = track.Features.First().Geometry.Type,
                                Coordinates = coordinates
                            }
                        }
                    }
                };

                new JsonWriter<Track>().Write(folder, compressedTrack, FileTypes.TrackCompressedJson, compressedTrack.Departure, compressedTrack.Arrival);
                Console.WriteLine($" - {track.Features.First().Geometry.Coordinates.Count} --> {coordinates.Count}");
            }
        }
    }
}
