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
            var flight = new SqlReader().Read(stkpDbFile, departure, arrival);
            flight.FileType = FileTypes.SimToolkitProJson;

            var landing = flight.Landing;
            landing.FileType = FileTypes.LandingJson;

            var logbook = flight.Logbook;
            logbook.FileType = FileTypes.LogbookJson;

            var track = new TrackExtracter().Extract(flightFolder);
            track.FileType = FileTypes.TrackJson;

            new JsonWriter<SimToolkitProFlight>().Write(flightFolder, flight, FileTypes.SimToolkitProJson, departure, arrival);
            new JsonWriter<Landing>().Write(flightFolder, landing, FileTypes.LandingJson, departure, arrival);
            new JsonWriter<Logbook>().Write(flightFolder, logbook, FileTypes.LogbookJson, departure, arrival);
            new JsonWriter<Track>().Write(flightFolder, track, FileTypes.TrackJson, departure, arrival);

            return flight;
        }
    }
}
