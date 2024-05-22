using System.Data;
using System.Globalization;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;

namespace TheFipster.Aviation.CoreCli
{
    public class FileOperations : IFileOperations
    {
        public string GetLatestFile(string folder, string searchPattern = null)
        {
            IEnumerable<string> filepaths = Enumerable.Empty<string>();
            if (string.IsNullOrWhiteSpace(searchPattern))
                filepaths = Directory.GetFiles(folder);
            else
                filepaths = Directory.GetFiles(folder, searchPattern);

            if (!filepaths.Any())
                throw new EmptyResultException($"Folder {folder} is empty.");

            var files = filepaths.Select(path => new FileInfo(path));
            var orderedFiles = files.OrderByDescending(file => file.LastWriteTime);
            var latestFile = orderedFiles.FirstOrDefault();
            return latestFile.FullName;
        }

        public void MoveScreenshots(IEnumerable<string> files, string path, string departure, string arrival, bool overwrite = false)
        {
            var fileInfos = files.Select(x => new FileInfo(x));
            int i = 0;
            foreach (var file in fileInfos.OrderBy(x => x.LastWriteTimeUtc))
            {
                i++;
                var filename = $"{departure} - {arrival} - Screenshot - {i}.png";
                var newFile = Path.Combine(path, filename);
                File.Move(file.FullName, newFile, overwrite);
            }
        }

        public void MoveFiles(IEnumerable<string> files, string path, bool overwrite = false)
        {
            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);
                var newFile = Path.Combine(path, filename);
                File.Move(file, newFile, overwrite);
            }
        }

        public ICollection<string> MoveFiles(string oldFolder, string newFolder, string searchPattern = null)
        {
            var newFiles = new List<string>();
            IEnumerable<string> files;
            if (string.IsNullOrWhiteSpace(searchPattern))
                files = Directory.GetFiles(oldFolder);
            else
                files = Directory.GetFiles(oldFolder, searchPattern);

            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);
                var newFile = Path.Combine(newFolder, filename);
                File.Move(file, newFile);
                newFiles.Add(newFile);
            }

            return newFiles;
        }

        public string CreateFlightFolder(string flightPlanFile, string flightsFolder, string departure, string arrival)
        {
            var plan = new JsonReader<IEnumerable<Leg>>().FromFile(flightPlanFile);

            var leg = plan.FirstOrDefault(x => x.From == departure && x.To == arrival);
            if (leg == null)
                throw new InvalidFlightException(departure, arrival, "The leg doesn't exist on the flight plan.");

            var flightNo = leg.No;
            var flightTerminators = $"{departure} - {arrival}";
            var flightName = $"{flightNo:D4} - {flightTerminators}";
            var flightPath = Path.Combine(flightsFolder, flightName);

            if (Directory.Exists(flightPath))
                throw new DuplicateFlightException(departure, arrival);

            if (Directory.GetDirectories(flightsFolder, $"*{flightTerminators}").Any())
                throw new DuplicateFlightException(departure, arrival);

            Directory.CreateDirectory(flightPath);
            return flightPath;
        }

        public IEnumerable<string> ScanForFiles(string path, string search = null, int intervalMs = 1000)
        {
            while (true)
            {
                var files = Directory.GetFiles(path, search);
                if (files.Any())
                    return files;

                Thread.Sleep(1000);
            }
        }

        public string CreateFlightFolder(string path, Leg leg)
        {
            var flightFolder = $"{leg.No:d4} - {leg.From} - {leg.To}";
            var flightPath = Path.Combine(path, flightFolder);
            if (!Directory.Exists(flightPath))
                Directory.CreateDirectory(flightPath);

            CreateInitFile(flightPath, leg);

            return flightPath;
        }

        public string CreateInitFile(string flightPath, Leg leg, DateTime? utcDate = null)
        {
            var createdFilename = $"{leg.From} - {leg.To} - Created.txt";
            var createdFile = Path.Combine(flightPath, createdFilename);

            if (!File.Exists(createdFile))
                File.Create(createdFile).Dispose();

            if (utcDate == null)
                utcDate = DateTime.UtcNow;

            var timeCreated = utcDate.Value.ToString(
                "yyyy-MM-ddTHH:mm:ssZ",
                CultureInfo.InvariantCulture);

            File.AppendAllText(
                createdFile,
                timeCreated + Environment.NewLine);

            return createdFile;
        }

        public IEnumerable<string> CollectFiles(string path, string search)
        {
            Console.WriteLine($"Collecting files from {path}");
            Console.WriteLine();

            while (true)
            {
                var files = Directory.GetFiles(path, search);
                foreach(var file in files)
                {
                    Console.WriteLine($"\t {file}");
                }

                Console.WriteLine();
                var confirmation = StdOut.YesNoDecision("Does this seem ok? ", true);
                Console.WriteLine();
                if (confirmation)
                    return files;
            }
        }
    }
}
