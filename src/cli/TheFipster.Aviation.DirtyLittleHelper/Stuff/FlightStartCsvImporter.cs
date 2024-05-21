using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.DirtyLittleHelper.Models;
using TheFipster.Aviation.Domain.Enums;
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
                var flightFile = new FlightFileScanner().GetFile(folder, FileTypes.FlightJson);
                var flight = new JsonReader<FlightImport>().FromFile(flightFile);
                flight.Started = item.Date.ToUniversalTime();
                new JsonWriter<FlightImport>().Write(flightFile, flight, true);
            }
        }
    }
}
