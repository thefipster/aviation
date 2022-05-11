using CommandLine;
using TheFipster.Aviation.FlightCli.Commands;
using TheFipster.Aviation.FlightCli.Options;


args = new string[] { "stkp", "-f", @"C:\Users\felix\Downloads\export.json" };
//args = new string[] { "rec", "-d", "KDEN", "-a", "KTEX" };

Parser.Default.ParseArguments<RecorderOptions, SimToolkitProImportOptions>(args)
            .WithParsed<RecorderOptions>(o =>
            {
                new RecorderCommand().Run(o);
            })
            .WithParsed<SimToolkitProImportOptions>(o =>
            {
                new SimToolkitProImportCommand().Run(o);
            })
            .WithNotParsed((o) => Console.WriteLine("Dude, what was that. I can't work like this."));