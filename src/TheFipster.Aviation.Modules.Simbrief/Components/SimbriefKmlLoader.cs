using System.Globalization;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Modules.Simbrief.Components
{
    public class SimbriefKmlLoader
    {
        public Route Load(string flightFolder)
        {
            var xmlFiles = new FileSystemFinder().GetFiles(flightFolder, FileTypes.SimbriefXml);
            if (!xmlFiles.Any())
                throw new ApplicationException("Simbrief xml data file not found.");

            var flight = new SimbriefXmlLoader().Read(xmlFiles.First());

            var files = new FileSystemFinder().GetFiles(flightFolder, FileTypes.RouteKml);
            if (!files.Any())
                throw new ApplicationException("Simbrief kml route file not found.");

            var route = new Route(flight.Departure.Icao, flight.Arrival.Icao);
            route.FileType = FileTypes.RouteJson;

            var file = files.First();
            var lines = File.ReadAllLines(file);

            bool skip = true;
            bool record = false;

            foreach (var line in lines)
            {
                if (line.Contains("<LineString>"))
                    skip = false;

                if (skip)
                    continue;

                if (line.Contains("<coordinates>"))
                {
                    record = true;
                    continue;
                }

                if (line.Contains("</coordinates>"))
                    break;

                if (record)
                {
                    var split = line.Split(',');
                    var latitude = double.Parse(split[0], CultureInfo.InvariantCulture);  
                    var longitude = double.Parse(split[1], CultureInfo.InvariantCulture);
                    var altitude = int.Parse(split[2]);
                    var coordinate = new Coordinate(latitude, longitude, altitude);
                    route.Coordinates.Add(coordinate);
                }
            }

            return route;
        }
    }
}
