using CommandLine;
using TheFipster.Aviation.Domain;
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
args = new[] { "scan" };
//args = new[] { "track" };
//args = new[] { "land" };
//args = new[] { "ports" };

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
    var config = new Configuration().Load();
    var newConfig = new HardcodedConfig();

    Parser.Default.ParseArguments<
            AirportOptions,
            FinishOptions,
            MergeOptions,
            RecorderOptions,
            SimbriefImportOptions,
            TrackOptions,
            WizardOptions,
            ScanOptions,
            LandingOptions>(args)
        .WithParsed<AirportOptions>(o => { new AirportCommand(newConfig).Run(o); })
        .WithParsed<FinishOptions>(o => { new FinishCommand().Run(o); })
        .WithParsed<MergeOptions>(o => { new MergeCommand().Run(o); })
        .WithParsed<RecorderOptions>(o => { new RecorderCommand().Run(o); })
        .WithParsed<SimbriefImportOptions>(o => { new SimbriefImportCommand(config).Run(o); })
        .WithParsed<TrackOptions>(o => { new TrackCommand(newConfig).Run(o); })
        .WithParsed<WizardOptions>(o => { new WizardCommand(newConfig).Run(o); })
        .WithParsed<ScanOptions>(o => { new ScanCommand(newConfig).Run(o); })
        .WithParsed<LandingOptions>(o => { new LandingCommand(newConfig).Run(o); })
        .WithNotParsed(_ => Console.Write(string.Empty));
}

