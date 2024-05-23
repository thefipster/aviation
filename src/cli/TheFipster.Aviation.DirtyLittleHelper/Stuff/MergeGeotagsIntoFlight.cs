using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Geo;

namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    public class MergeGeotagsIntoFlight
    {
        public void Do()
        {
            var flightsFolder = "E:\\aviation\\Worldtour\\Flights";
            var folders = Directory.GetDirectories(flightsFolder);

            foreach (var folder in folders)
            {
                try
                {
                    appendGeoTagsToFlight(folder);
                }
                catch (FileNotFoundException)
                {
                    // no geotags
                    continue;
                }
            }
        }

        private void appendGeoTagsToFlight(string folder)
        {
            var flightFile = new FlightFileScanner().GetFile(folder, FileTypes.FlightJson);
            var flight = new JsonReader<FlightImport>().FromFile(flightFile);

            var geotagsFile = new FlightFileScanner().GetFile(folder, FileTypes.GeoTagsJson);
            var geotags = new JsonReader<GeoTagReport>().FromFile(geotagsFile);

            if (geotags?.GeoTags == null || !geotags.GeoTags.Any())
                return;

            if (flight.Geotags == null || !flight.Geotags.Any())
                flight.Geotags = geotags.GeoTags;

            new JsonWriter<FlightImport>().Write(flightFile, flight, true);
        }
    }
}
