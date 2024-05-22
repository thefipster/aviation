using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class SimbriefDownloadCommand : IFlightRequiredCommand<SimbriefDownloadOptions>
    {
        private readonly SimbriefDownloader downloader;

        public SimbriefDownloadCommand()
        {
            downloader = new SimbriefDownloader();
        }

        public void Run(SimbriefDownloadOptions options, IConfig config)
        {
            Console.WriteLine(SimbriefDownloadOptions.Welcome);
            Guard.EnsureConfig(config);

            var folder = options.GetFlightFolder(config.FlightsFolder);
            downloader.DownloadXml(folder, config.SimbriefPilotId);

            Thread.Sleep(1000);

            downloader.DownloadKml(folder);
            downloader.DownloadPdf(folder);
            downloader.DownloadMaps(folder);
        }
    }
}
