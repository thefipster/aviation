using CommandLine;
using TheFipster.Aviation.FlightCli.Commands;
using TheFipster.Aviation.FlightCli.Options;


//args = new [] { "simbrief", "-f", @"C:\Users\freisch\Aviation\test\simbrief_flightplan.xml" };
//args = new [] { "stkp", "-f", @"C:\Users\freisch\Aviation\test\simtoolkitpro_export.json" };
//args = new [] { "rec", "-d", "KDEN", "-a", "KTEX" };
//args = new [] { "airpt", "-i", "KCOS" };
//args = new [] { "woop" };
args = new[]
{
    "merge",
    "-a", @"C:\Users\freisch\Aviation\flight\KDEN - Airport.json",
     @"C:\Users\freisch\Aviation\flight\KTEX - Airport.json",
     @"C:\Users\freisch\Aviation\flight\KCOS - Airport.json",
    "-b", @"C:\Users\freisch\Aviation\flight\KDEN - KTEX - BlackBox.json",
    "-s", @"C:\Users\freisch\Aviation\flight\KDEN - KTEX - Simbrief.json",
    "-t", @"C:\Users\freisch\Aviation\flight\KDEN - KTEX - SimToolkitPro.json",
};

Parser.Default.ParseArguments<
        AirportDetailsOptions,
        MergeOptions,
        RecorderOptions, 
        SimbriefImportOptions,
        SimToolkitProImportOptions>(args)
    .WithParsed<AirportDetailsOptions>(o => { new AirportDetailsCommand().Run(o); })
    .WithParsed<MergeOptions>(o => { new MergeCommand().Run(o); })
    .WithParsed<RecorderOptions>(o => { new RecorderCommand().Run(o); })
    .WithParsed<SimbriefImportOptions>(o => { new SimbriefImportCommand().Run(o); })
    .WithParsed<SimToolkitProImportOptions>(o => { new SimToolkitProImportCommand().Run(o); })
    .WithNotParsed(_ => Console.WriteLine("Why so weak? Why?"));