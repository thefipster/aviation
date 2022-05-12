using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.SimToolkitPro;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class SimToolkitProImportCommand
    {
        internal void Run(SimToolkitProImportOptions options)
        {
            var flights = new Importer().Read(options.Filepath);
            var writer = new JsonWriter<SimToolkitProFlight>();

            foreach (var flight in flights)
                writer.Write(
                    flight, 
                    "SimToolkitPro", 
                    flight.Logbook.Dep, 
                    flight.Logbook.Arr);
        }
    }
}
