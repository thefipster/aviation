namespace TheFipster.Aviation.CoreCli.Abstractions
{
    public interface IFlightFinder
    {
        string GetFlightFolder(string flightsFolder, string departure, string arrival);

        IEnumerable<string> GetFlightFolders(string flightsFolder);
    }
}
