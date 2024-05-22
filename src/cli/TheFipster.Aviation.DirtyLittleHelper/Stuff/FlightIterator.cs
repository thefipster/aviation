using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    internal class FlightIterator
    {
        public void Do()
        {
            var flightsFolder = "E:\\aviation\\Worldtour\\Flights";
            var folders = new FlightFinder().GetFlightFolders(flightsFolder);

            foreach (var folder in folders)
            {
                var flightFile = new FlightFileScanner().GetFile(folder, FileTypes.FlightJson);
                var flight = new JsonReader<FlightImport>().FromFile(flightFile);

                Console.WriteLine(flight.Departure + " - " + flight.Arrival);
            }
        }
    }
}
