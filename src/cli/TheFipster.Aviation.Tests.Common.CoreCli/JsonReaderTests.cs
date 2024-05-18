using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Geo;

namespace TheFipster.Aviation.Tests.Common.CoreCli
{
    public class JsonReaderTests
    {
        [Fact]
        public void Test1()
        {
            var reader = new JsonReader<GpsReport>();
            var report = new GpsReport
            {
                Arrival = "EDDL",
                Departure = "EGLL",

            };
        }
    }
}