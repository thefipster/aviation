namespace TheFipster.Aviation.Modules.SimToolkitPro.Components
{
    public class Finder
    {
        public FileInfo Find(string directory)
        {
            var filepaths = Directory.GetFiles(directory, "*.json");
            var files = filepaths.Select(path => new FileInfo(path));
            var orderedFiles = files.OrderByDescending(file => file.LastWriteTime);
            var latestFile = orderedFiles.FirstOrDefault();

            if (latestFile == null)
                throw new ApplicationException($"Couldn't find an export file in the toolkit folder located at {directory}");

            return latestFile;
        }
    }
}
