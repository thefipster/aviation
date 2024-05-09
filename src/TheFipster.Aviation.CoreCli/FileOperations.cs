using TheFipster.Aviation.CoreCli.Abstractions;
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
                throw new EmptyResultException("Folder is empty.");

            var files = filepaths.Select(path => new FileInfo(path));
            var orderedFiles = files.OrderByDescending(file => file.LastWriteTime);
            var latestFile = orderedFiles.FirstOrDefault();
            return latestFile.FullName;
        }
    }
}
