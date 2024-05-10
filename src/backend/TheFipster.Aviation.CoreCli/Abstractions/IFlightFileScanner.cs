using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.CoreCli.Abstractions
{
    public interface IFlightFileScanner
    {
        Dictionary<string, FileTypes> GetFiles(string flightFolder);

        IEnumerable<string> GetFiles(string flightFolder, FileTypes filetype);

        string GetFile(string flightFolder, FileTypes filetype);
    }
}
