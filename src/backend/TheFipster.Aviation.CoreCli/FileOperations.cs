using System.Data;
using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
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
    }
}
