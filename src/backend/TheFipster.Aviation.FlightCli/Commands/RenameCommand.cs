using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Simbrief.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class RenameCommand
    {
        private HardcodedConfig config;

        public RenameCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(RenameOptions options)
        {
            Console.WriteLine("Renaming imported files to follow proper format. This means Simbrief files and screenshots.");
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];


            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");

                new SimbriefRenamer().Rename(folder);

                var departure = new FlightMeta().GetDeparture(folder);
                var arrival = new FlightMeta().GetArrival(folder);
                var screenshots = new FlightFileScanner().GetFiles(folder, Domain.Enums.FileTypes.Screenshot).Select(x => new FileInfo(x));
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
