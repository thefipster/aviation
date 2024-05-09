using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Modules.Simbrief.Models;

namespace TheFipster.Aviation.Modules.Simbrief.Components
{
    public class SimbriefXmlSplitter
    {
        public SplitResult Split(string flightFolder)
        {
            var files = new FileSystemFinder().GetFiles(flightFolder, FileTypes.SimbriefXml);
            if (!files.Any())
                throw new ApplicationException("Couldn't find simbrief xml data file.");

            var result = new SplitResult();

            var flight = new SimbriefXmlLoader().Read(files.First());
            flight.FileType = FileTypes.SimbriefJson;
            result.Flight = flight;

            var waypoints = new SimbriefWaypoints(flight.Departure.Icao, flight.Arrival.Icao);
            waypoints.Waypoints = flight.Waypoints;
            flight.Waypoints = new List<Waypoint>();
            result.Waypoints = waypoints;

            var notams = new SimbriefXmlLoader().ReadNotams(files.First());
            notams.FileType = FileTypes.NotamJson;
            result.Notams = notams;

            var ofp = new SimbriefXmlLoader().ReadOfp(files.First());
            result.Ofp = ofp;

            return result;
        }
    }
}
