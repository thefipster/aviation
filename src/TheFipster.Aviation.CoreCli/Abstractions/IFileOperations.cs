
namespace TheFipster.Aviation.CoreCli.Abstractions
{
    public interface IFileOperations
    {
        string GetLatestFile(string folder, string searchPattern = null);
        IEnumerable<string> MoveFiles(string oldFolder, string newFolder, string searchPattern = null);
    }
}
