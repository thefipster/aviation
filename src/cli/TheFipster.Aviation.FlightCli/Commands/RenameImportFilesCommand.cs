using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class RenameImportFilesCommand : IFlightCommand<RenameImportFilesOptions>
    {
        public void Run(RenameImportFilesOptions options, IConfig config)
        {
            Console.WriteLine("Renaming imported files to follow proper format. This means Simbrief files and screenshots.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");

                new SimbriefRenamer().Rename(folder);

                var departure = new FlightMeta().GetDeparture(folder);
                var arrival = new FlightMeta().GetArrival(folder);

                var pngs = Directory.GetFiles(folder, "*.png");
                var charts = new FlightFileScanner().GetFiles(folder, FileTypes.ChartPrint);
                var screenshots = pngs.Except(charts).Select(x => new FileInfo(x));

                int i = 0;
                foreach (var screenshot in screenshots.OrderBy(x => x.LastWriteTime))
                {
                    i++;
                    var oldName = Path.GetFileName(screenshot.FullName);
                    var newName = $"{departure} - {arrival} - Screenshot - {i}.png";
                    var newFile = Path.Combine(folder, newName);

                    if (oldName == newName)
                        continue;

                    File.Move(screenshot.FullName, newFile);
                }
            }
        }
    }
}
