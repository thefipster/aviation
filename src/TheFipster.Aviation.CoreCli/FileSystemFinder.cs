namespace TheFipster.Aviation.CoreCli
{
    public class FileSystemFinder
    {
        public string GetFlightFolder(string flightsFolder, string departure, string arrival)
        {
            var search = $"* - {departure} - {arrival}";
            var candidates = Directory.GetDirectories(flightsFolder, search);
            if (!candidates.Any())
                throw new ApplicationException($"Couldn't find flight from {departure} to {arrival}.");

            return candidates.First();
        }
    }
}
