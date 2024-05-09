using TheFipster.Aviation.CoreCli.Abstractions;

namespace TheFipster.Aviation.CoreCli
{
    public class FlightMeta : IFlightMeta
    {
        public int GetLeg(string flightFolder)
        {
            var folder = Path.GetFileName(flightFolder);
            var split = folder.Split('-');
            return int.Parse(split[0].Trim());
        }

        public string GetDeparture(string flightFolder)
        {
            var folder = Path.GetFileName(flightFolder);
            var split = folder.Split('-');
            return split[1].Trim();
        }

        public string GetArrival(string flightFolder)
        {
            var folder = Path.GetFileName(flightFolder);
            var split = folder.Split('-');
            return split[2].Trim();
        }
    }
}
