using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Started");

        // do your dirty things
        var flightsFolder = "E:\\aviation\\Worldtour\\Flights";
        var folders = new FlightFinder().GetFlightFolders(flightsFolder);

        foreach (var folder in folders)
        {
            var flightFile = new FlightFileScanner().GetFile(folder, FileTypes.FlightJson);
            var flight = new JsonReader<FlightImport>().FromFile(flightFile);
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(flight.SimbriefXml.Ofp.Params.TimeGenerated)).UtcDateTime;

            Console.WriteLine(flight.Departure + " - " + flight.Arrival + " --- " + flight.Started + " | " + timestamp + " ==> " + (timestamp - flight.Started));
        }

        Console.WriteLine("Finished");
    }
}