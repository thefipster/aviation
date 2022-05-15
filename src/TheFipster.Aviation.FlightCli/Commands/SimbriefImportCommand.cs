using Microsoft.Extensions.Configuration;
using TheFipster.Aviation.FlightCli.Models;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class SimbriefImportCommand
    {
        private readonly FolderConfig config;

        public SimbriefImportCommand(IConfiguration config)
        {
            this.config = config.GetSection("folders").Get<FolderConfig>();
            if (string.IsNullOrWhiteSpace(this.config.SimbriefFolder))
                throw new ApplicationException("Simbrief export folder is not specified in the appsettings.json");
        }

        internal void Run(SimbriefImportOptions options) => new Importer().Load(config.SimbriefFolder);
    }
}
