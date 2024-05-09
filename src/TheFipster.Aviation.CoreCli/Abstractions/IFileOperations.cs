namespace TheFipster.Aviation.CoreCli.Abstractions
{
    public interface IFileOperations
    {
        string GetLatestFile(string folder, string searchPattern = null);
    }
}
