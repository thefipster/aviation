using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.CoreCli.Abstractions
{
    public interface IFileSystemFinder
    {
        string GetFlightFolder(string flightsFolder, string departure, string arrival);

        IEnumerable<string> GetFlightFolders(string flightsFolder);

        Dictionary<string, FileTypes> GetFiles(string flightFolder);

        IEnumerable<string> GetFiles(string flightFolder, FileTypes filetype);

        int GetLeg(string flightFolder);

        string GetDeparture(string flightFolder);

        string GetArrival(string flightFolder);
    }
}
