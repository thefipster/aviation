using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.DirtyLittleHelper.Models;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    internal class FlightStartCsvImporter
    {
        public void Do()
        {
            var flightdateFile = "E:\\aviation\\Worldtour\\FlightsFiled.csv";
            var flightsFolder = "E:\\aviation\\Worldtour\\Flights";
            var data = new CsvReader().FromFile(flightdateFile, ";");

            foreach (var line in data)
            {
                var item = new FlightFiled(line);
                Console.WriteLine(item.Departure + " - " + item.Arrival);
                var folder = new FlightFinder().GetFlightFolder(flightsFolder, item.Departure, item.Arrival);
                var departure = new FlightMeta().GetDeparture(folder);
                var arrival = new FlightMeta().GetArrival(folder);
                var flightNo = new FlightMeta().GetLeg(folder);

                var leg = new Leg(flightNo, departure, arrival);
                var file = new FileOperations().CreateInitFile(folder, leg, item.Date.ToUniversalTime());
                Console.WriteLine("\t" + file);
            }
        }
    }
}
