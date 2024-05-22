using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    public class GetImageTakenFromGeotags
    {
        public void Do()
        {
            var flightsFolder = "E:\\aviation\\Worldtour\\Flights";
            var folders = Directory.GetDirectories(flightsFolder);

            foreach (var folder in folders)
            {
                var flightFile = new FlightFileScanner().GetFile(folder, FileTypes.FlightJson);
                var flight = new JsonReader<FlightImport>().FromFile(flightFile);

                if (flight.Geotags == null || !flight.Geotags.Any())
                    continue;

                
            }

        }
    }
}
