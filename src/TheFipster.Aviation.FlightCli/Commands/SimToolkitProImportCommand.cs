using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.SimToolkitPro;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class SimToolkitProImportCommand
    {
        internal void Run(SimToolkitProImportOptions options) => new Importer().Read(options.Filepath);
    }
}
