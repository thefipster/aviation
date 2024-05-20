using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.SimToolkitPro;

namespace TheFipster.Aviation.Modules.SimToolkitPro.Components
{
    public class SimToolkitProCompressor
    {
        public Track CompressTrack(Track track)
        {
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

            return compressedTrack;
        }
    }
}
