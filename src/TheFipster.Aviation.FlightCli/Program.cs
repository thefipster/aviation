using CommandLine;
using TheFipster.Aviation.FlightCli.Commands;
using TheFipster.Aviation.FlightCli.Options;


//args = new [] { "simbrief", "-f", @"C:\Users\freisch\Aviation\test\simbrief_flightplan.xml" };
//args = new [] { "stkp", "-f", @"C:\Users\freisch\Aviation\test\simtoolkitpro_export.json" };
//args = new [] { "rec", "-d", "KDEN", "-a", "KTEX" };
//args = new [] { "airpt", "-i", "KDEN" };
//args = new [] { "woop" };

Parser.Default.ParseArguments<
        AirportDetailsOptions,
        RecorderOptions, 
        SimbriefImportOptions,
        SimToolkitProImportOptions>(args)
    .WithParsed<AirportDetailsOptions>(o => { new AirportDetailsCommand().Run(o); })
    .WithParsed<RecorderOptions>(o => { new RecorderCommand().Run(o); })
    .WithParsed<SimbriefImportOptions>(o => { new SimbriefImportCommand().Run(o); })
    .WithParsed<SimToolkitProImportOptions>(o => { new SimToolkitProImportCommand().Run(o); })
    .WithNotParsed(_ => Console.WriteLine("Why so weak? Why?"));