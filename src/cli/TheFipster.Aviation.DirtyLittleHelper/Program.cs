using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.DirtyLittleHelper.Stuff;
using TheFipster.Aviation.FlightCli;
using TheFipster.Aviation.Modules.Simbrief;
using TheFipster.Aviation.Modules.SimConnectClient.Components;

internal class Program
{
    private const string UrlFormatXml = "https://www.simbrief.com/api/xml.fetcher.php?userid={0}";
    private const string UrlFormatJson = "https://www.simbrief.com/api/xml.fetcher.php?userid={0}&json=v2";

    private static void Main(string[] args)
    {
        Console.WriteLine("Started");
        Console.WriteLine();

        // do your dirty little things here
        var config = new HardcodedConfig();
        var jsonUrl = string.Format(UrlFormatJson, config.SimbriefPilotId);
        var filepath = "E:\\aviation\\Worldtour\\test.json";
        new FileDownloader().Download(jsonUrl, filepath);

        Console.WriteLine();
        Console.WriteLine("Finished");
    }
}