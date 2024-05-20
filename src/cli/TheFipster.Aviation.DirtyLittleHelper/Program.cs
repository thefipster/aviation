using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Simbrief;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Started");

        // do your dirty things
        var simbriefJson = "E:\\aviation\\Worldtour\\Flights\\0044 - UHPP - UHSI\\UHPP - UHSI - SimbriefImport.json";
        var import = new JsonReader<SimbriefImport>().FromFile(simbriefJson);

        var expiredDatesOrigin = import.Origin.Notam.Select(x => x.DateExpire.ToString()).ToList();
        var expiredDatesDestination = import.Destination.Notam.Select(x => x.DateExpire.ToString()).ToList();

        Console.WriteLine("Finished");
    }
}