using Microsoft.Extensions.Configuration;
using TheFipster.Aviation.FlightCli.Models;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.SimToolkitPro;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class SimToolkitProImportCommand
    {
        private readonly FolderConfig config;

        internal SimToolkitProImportCommand(IConfiguration config)
        {
            this.config = config.GetSection("folders").Get<FolderConfig>();
            if (string.IsNullOrWhiteSpace(this.config.ToolkitFolder))
                throw new ApplicationException("SimToolkitPro export folder is not specified in the appsettings.json");
        }

        internal void Run(SimToolkitProImportOptions options) => new Importer().Load(config.ToolkitFolder);
    }
}
