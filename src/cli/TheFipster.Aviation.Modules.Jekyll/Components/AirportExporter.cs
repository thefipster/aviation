using System.Collections.Concurrent;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Modules.Airports.Components;
using TheFipster.Aviation.Modules.Jekyll.Model;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    internal class AirportExporter
    {
        public List<Location> GetPlannedAirports(string flightsFolder, OurAirportFinder airports)
        {
            var result = new ConcurrentBag<Location>();
            var folders = Directory.GetDirectories(flightsFolder);
            Parallel.ForEach(folders, folder =>
            {
                var flightFile = new FlightFileScanner().GetFile(folder, FileTypes.FlightJson);
                var flight = new JsonReader<FlightImport>().FromFile(flightFile);

                var departure = airports.SearchWithIcao(flight.Departure);
                result.Add(new Location(departure));

                var arrival = airports.SearchWithIcao(flight.Arrival);
                result.Add(new Location(arrival));

                if (flight.HasSimbriefXml && flight.SimbriefXml.Ofp.Alternate != null)
                {
                    var alternate = airports.SearchWithIcao(flight.SimbriefXml.Ofp.Alternate.IcaoCode);
                    result.Add(new Location(alternate));
                }
            });

            return result.ToList();
        }
    }
}
