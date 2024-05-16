using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.Simbrief.Components
{
    public class SimbriefRenamer
    {
        private SimBriefFlight flight;

        public void Rename(string flightFolder)
        {
            var file = new FlightFileScanner().GetFile(flightFolder, Domain.Enums.FileTypes.SimbriefJson);
            flight = new JsonReader<SimBriefFlight>().FromFile(file);

            var oldFiles = Directory.GetFiles(flightFolder, $"{flight.Departure.Icao}{flight.Arrival.Icao}*");
            renameFile(flightFolder, oldFiles, ".xml", "Simbrief");
            renameFile(flightFolder, oldFiles, ".pdf", "OFP");
            renameFile(flightFolder, oldFiles, ".kml", "Route");
            renameFile(flightFolder, oldFiles, ".pln", "FlightPlan");
        }

        private void renameFile(string flightFolder, string[] oldFiles, string extension, string name)
        {
            var oldFile = oldFiles.FirstOrDefault(x => x.Contains(extension));
            if (oldFile == null)
                return;

            var newName = $"{flight.Departure.Icao} - {flight.Arrival.Icao} - {name}{extension}";
            var oldName = Path.GetFileName(oldFile);

            if (oldName == newName)
                return;

            var newFile = Path.Combine(flightFolder, newName);
            File.Move(oldFile, newFile);
        }
    }
}
