namespace TheFipster.Aviation.Modules.Simbrief.Components
{
    public class SimbriefFinder
    {
        public IEnumerable<string> FindExportFiles(string directory, string departureICAO, string arrivalICAO) 
            => Directory.GetFiles(directory, $"{departureICAO}{arrivalICAO}*");
    }
}
