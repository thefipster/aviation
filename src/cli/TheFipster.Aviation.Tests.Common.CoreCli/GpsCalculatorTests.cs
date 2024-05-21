using FluentAssertions;
using TheFipster.Aviation.CoreCli;

namespace TheFipster.Aviation.Tests.Common.CoreCli
{
    public class GpsCalculatorTests
    {
        [Fact]
        public void GetHaversineDistanceLongTest()
        {
            var actualDistance = 5574.8;

            var lat1 = 51.5007;
            var lon1 = 0.1246;
            var lat2 = 40.6892;
            var lon2 = 74.0445;

            var distance = GpsCalculator.GetHaversineDistance(lat1, lon1, lat2, lon2);

            distance.Should().BeApproximately(actualDistance, actualDistance * 0.001);
        }

        [Fact]
        public void GetHaversineDistanceShortTest()
        {
            var actualDistance = 0.03173;

            var lat1 = 51.178991;
            var lon1 = -1.826252;
            var lat2 = 51.178708;
            var lon2 = -1.826193;

            var distance = GpsCalculator.GetHaversineDistance(lat1, lon1, lat2, lon2);

            distance.Should().BeApproximately(actualDistance, actualDistance * 0.001);
        }

        [Fact]
        public void GetBearingHeathrow27RTest()
        {
            var actualBearing = 269.6;

            var lat1 = 51.477699279785156;
            var lon1 = -0.43326398730278015;
            var lat2 = 51.477500915527344;
            var lon2 = -0.48942801356315613;

            var bearing = GpsCalculator.GetBearing(lat1, lon1, lat2, lon2);

            bearing.Should().BeApproximately(actualBearing, actualBearing * 0.01);
        }

        [Fact]
        public void GetBearingPetersburg05Test()
        {
            var actualBearing = 70.7;

            var lat1 = 56.79890060424805;
            var lon1 = -132.95899963378906;
            var lat2 = 56.80440139770508;
            var lon2 = -132.93099975585938;

            var bearing = GpsCalculator.GetBearing(lat1, lon1, lat2, lon2);

            bearing.Should().BeApproximately(actualBearing, actualBearing * 0.01);
        }

        [Fact]
        public void GetCrosstrackErrorTest()
        {
            var actualCrosstrackError = 0.554d;

            var latA = 23.563276;
            var lonA = 3.336203;
            var latB = 23.558573;
            var lonB = 3.397407;
            var latC = 23.555330;
            var lonC = 3.375001;

            var crosstrackError = GpsCalculator.GetCrossTrack(latA, lonA, latB, lonB, latC, lonC);

            crosstrackError.Should().BeApproximately(actualCrosstrackError, actualCrosstrackError * 0.01);
        }

        [Fact]
        public void GetCrosstrackError2Test()
        {
            var actualCrosstrackError = 0.307d;

            var latA = 53.3206;
            var lonA = -1.7297;
            var latB = 53.1887;
            var lonB = 0.1334;
            var latC = 53.2611;
            var lonC = -0.7972;

            var crosstrackError = GpsCalculator.GetCrossTrack(latA, lonA, latB, lonB, latC, lonC);

            crosstrackError.Should().BeApproximately(actualCrosstrackError, actualCrosstrackError * 0.01);
        }
    }
}
