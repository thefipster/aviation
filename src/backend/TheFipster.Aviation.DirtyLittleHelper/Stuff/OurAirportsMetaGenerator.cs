using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    internal class OurAirportsMetaGenerator
    {
        public void Do(string importPath, string exportPath)
        {
            var airportFiles = Directory.GetFiles(importPath);

            Parallel.ForEach(airportFiles, file =>
            {
                var airports = new JsonReader<IEnumerable<OurAirport>>().FromFile(file);
                var metas = new List<AirportMeta>();

                foreach (var airport in airports)
                {
                    var meta = new AirportMeta(airport.Ident, airport.Type, airport.Latitude, airport.Longitude);
                    metas.Add(meta);
                }

                var filename = Path.GetFileName(file);
                var newFile = Path.Combine(exportPath, filename);
                new JsonWriter<IEnumerable<AirportMeta>>().Write(newFile, metas, true);
            });
        }

        
    }
}
