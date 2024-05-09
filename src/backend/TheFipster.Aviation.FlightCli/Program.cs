using CommandLine;
using TheFipster.Aviation.FlightCli;
using TheFipster.Aviation.FlightCli.Commands;
using TheFipster.Aviation.FlightCli.Options;

//args = new [] { "simbrief", "-f", @"C:\Users\felix\Aviation\flight\KDENKTEX_XML_1652208542.xml" };
//args = new [] { "rec", "-d", "EDDL", "-a", "EDDK" };
//args = new [] { "woop" };
//args = new [] { "--help" };
//args = new[] { "fin", "EDDL", "EDDK" };  
//args = new[]
//{
//    "merge",
//    "-a", @"C:\Users\felix\Aviation\flight\KDEN - Airport.json",
//          @"C:\Users\felix\Aviation\flight\KTEX - Airport.json",
//          @"C:\Users\felix\Aviation\flight\KCOS - Airport.json",
//    "-b", @"C:\Users\felix\Aviation\flight\KDEN - KTEX - BlackBox.json",
//    "-s", @"C:\Users\felix\Aviation\flight\KDEN - KTEX - Simbrief.json",
//    "-t", @"C:\Users\felix\Aviation\flight\KDEN - KTEX - SimToolkitPro.json",
//};
//args = new[] { "wizard" };
//args = new[] { "scan" };
//args = new[] { "track" };
//args = new[] { "land" };
//args = new[] { "ports" };
//args = new[] { "simbrief" };
//args = new[] { "notam" };
//args = new[] { "toolkit" };
//args = new[] { "kml" };
//args = new [] { "sql", "-d", "CYXT", "-a", "PAPG" };

try
{
    run();
    Console.WriteLine($"Finished");
}
catch (ApplicationException ex)
{
    Console.WriteLine();
    Console.WriteLine($"Whoopsie. {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine($"Exception");
    Console.WriteLine();
    Console.WriteLine(ex.GetType().Name);
    Console.WriteLine(ex.Message);
    Console.Write(ex.StackTrace);
    Console.WriteLine();
}

void run() {
    var config = new HardcodedConfig();

    Parser.Default.ParseArguments<
            AirportOptions,
            MergeOptions,
            RecorderOptions,
            SimbriefOptions,
            TrackOptions,
            WizardOptions,
            ScanOptions,
            LandingOptions,
            NotamOptions,
            SqlOptions,
            ToolkitOptions,
            KmlOptions>(args)
        .WithParsed<AirportOptions>(options => { new AirportCommand(config).Run(options); })
        .WithParsed<MergeOptions>(options => { new MergeCommand().Run(options); })
        .WithParsed<RecorderOptions>(options => { new RecorderCommand(config).Run(options); })
        .WithParsed<SimbriefOptions>(options => { new SimbriefCommand(config).Run(options); })
        .WithParsed<TrackOptions>(options => { new TrackCommand(config).Run(options); })
        .WithParsed<WizardOptions>(options => { new WizardCommand(config).Run(options); })
        .WithParsed<ScanOptions>(options => { new ScanCommand(config).Run(options); })
        .WithParsed<LandingOptions>(options => { new LandingCommand(config).Run(options); })
        .WithParsed<NotamOptions>(options => { new NotamCommand(config).Run(options); })
        .WithParsed<SqlOptions>(options => { new SqlCommand(config).Run(options); })
        .WithParsed<ToolkitOptions>(options => { new ToolkitCommand(config).Run(options); })
        .WithParsed<KmlOptions>(options => { new KmlCommand(config).Run(options); })
        .WithNotParsed(_ => Console.Write(string.Empty));
}

