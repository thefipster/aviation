using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.BlackBox;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Geo;
using TheFipster.Aviation.Modules.BlackBox.Components;

namespace TheFipster.Aviation.Modules.BlackBox
{
    public class BlackboxOperations
    {
        private readonly BlackBoxCompressor compressor;
        private readonly BlackBoxScanner scanner;
        private readonly BlackboxGeotagger geotagger;
        private readonly BlackBoxCsvWriter csvWriter;
        private readonly JsonWriter<BlackBoxFlight> jsonWriter;
        private readonly JsonReader<BlackBoxFlight> jsonReader;

        public BlackboxOperations()
        {
            compressor = new BlackBoxCompressor();
            scanner = new BlackBoxScanner();
            geotagger = new BlackboxGeotagger();
            csvWriter = new BlackBoxCsvWriter();
            jsonWriter = new JsonWriter<BlackBoxFlight>();
            jsonReader = new JsonReader<BlackBoxFlight>();
        }

        public ICollection<Record> Compress(ICollection<Record> records)
            => compressor.CompressRecords(records);

        public BlackBoxStats Scan(ICollection<Record> records)
            => scanner.GenerateStatsFromBlackbox(records);

        public GeoTagReport GeotagScreenshots(FlightImport flight, string flightFolder)
            => geotagger.GeocodeScreenshots(flight, flightFolder);

        public void WriteJson(string folder, BlackBoxFlight bbFlight, bool overwrite = false)
            => jsonWriter.Write(folder, bbFlight, FileTypes.BlackBoxJson, bbFlight.Origin, bbFlight.Destination, overwrite);

        public void ReadJson(string filepath)
            => jsonReader.FromFile(filepath);

        public void WriteCsv(string folder, BlackBoxFlight bbFlight, bool overwrite = false)
            => csvWriter.Write(folder, bbFlight, FileTypes.BlackBoxCsv, overwrite);
    }
}
