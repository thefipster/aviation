using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.Modules.SimToolkitPro.Components;

namespace TheFipster.Aviation.Modules.SimToolkitPro
{
    public class SimToolkitProImporter
    {
        public SimToolkitProFlight Import(string flightFolder, string stkpDbFile, string departure, string arrival)
        {
            var flight = new SimToolkitProSqlReader().Read(stkpDbFile, departure, arrival);
            new JsonWriter<SimToolkitProFlight>().Write(flightFolder, flight, FileTypes.SimToolkitProJson, departure, arrival);

            var logbook = flight.Logbook;
            new JsonWriter<Logbook>().Write(flightFolder, logbook, FileTypes.LogbookJson, departure, arrival);

            if (!string.IsNullOrWhiteSpace(flight.Landing.LocalId))
            {
                var landing = flight.Landing;
                new JsonWriter<Landing>().Write(flightFolder, landing, FileTypes.LandingJson, departure, arrival);
            }

            if (!string.IsNullOrWhiteSpace(logbook.TrackedGeoJson) && logbook.TrackedGeoJson != "[]")
            {
                var track = new SimToolkitProTrackExtracter().FromText(logbook.TrackedGeoJson, logbook.Dep, logbook.Arr);
                new JsonWriter<Track>().Write(flightFolder, track, FileTypes.TrackJson, departure, arrival);
            }

            return flight;
        }
    }
}
