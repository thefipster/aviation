using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Geo;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.Modules.SimToolkitPro.Components;

namespace TheFipster.Aviation.Modules.SimToolkitPro
{
    public class StkpOps
    {
        private readonly SimToolkitProCompressor compressor;
        private readonly SimToolkitProGeotagger geotagger;
        private readonly SimToolkitProScanner scanner;
        private readonly SimToolkitProSqlReader reader;
        private readonly SimToolkitProTrackExtracter trackmaker;
        private readonly JsonWriter<SimToolkitProFlight> writer;

        public StkpOps()
        {
            compressor = new SimToolkitProCompressor();
            geotagger = new SimToolkitProGeotagger();
            scanner = new SimToolkitProScanner();
            reader = new SimToolkitProSqlReader();
            trackmaker = new SimToolkitProTrackExtracter();
            writer = new JsonWriter<SimToolkitProFlight>();
        }

        public Track CompressTrack(Track track)
            => compressor.CompressTrack(track);

        public GeoTagReport GeotagScreenshots(FlightImport flight, string flightFolder)
            => geotagger.GeocodeScreenshots(flight, flightFolder);

        public LogbookStats ScanFlight(SimToolkitProFlight stkpFlight)
            => scanner.Scan(stkpFlight);

        public SimToolkitProFlight ReadFlight(string stkpUserDbFile, string departure, string arrival)
            => reader.Read(stkpUserDbFile, departure, arrival);

        public Track? ExtractTrack(SimToolkitProFlight stkpFlight)
            => trackmaker.FromText(stkpFlight?.Logbook?.TrackedGeoJson);

        public void WriteFlight(string folder, SimToolkitProFlight stkpFlight, bool overwrite = false)
            => writer.Write(folder, stkpFlight, FileTypes.SimToolkitProJson, stkpFlight.Logbook.Dep, stkpFlight.Logbook.Arr, overwrite);
    }
}
