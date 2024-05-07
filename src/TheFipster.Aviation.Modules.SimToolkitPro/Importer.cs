using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Modules.SimToolkitPro.Components;

namespace TheFipster.Aviation.Modules.SimToolkitPro
{
    public class Importer
    {
        private readonly Finder searcher;
        private readonly Loader importer;
        private readonly JsonWriter<SimToolkitProFlight> writer;

        public Importer()
        {
            searcher = new Finder();
            importer = new Loader();
            writer = new JsonWriter<SimToolkitProFlight>();
        }

        public void Load(string folder)
        {
            var file = searcher.Find(folder);
            var flights = importer.Read(file);

            foreach (var flight in flights)
                writer.Write(
                    flight,
                    "SimToolkitPro",
                    flight.Logbook.Dep,
                    flight.Logbook.Arr);
        }
    }
}
