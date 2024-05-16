using CommandLine;
using System.Reflection;
using TheFipster.Aviation.FlightCli;
using TheFipster.Aviation.FlightCli.Commands;
using TheFipster.Aviation.FlightCli.Options;

//args = new [] { "simbrief", "-f", @"C:\Users\felix\Aviation\flight\KDENKTEX_XML_1652208542.xml" };
//args = new [] { "rec", "-d", "EDDL", "-a", "EDDK" };
//args = new [] { "--help" };
args = new[] { "wizard" };
//args = new[] { "scan" };
//args = new[] { "next" };
//args = new[] { "chart" };
//args = new[] { "preview", "-h", "300" };
//args = new[] { "rename", "-d", "CYXT", "-a", "PAPG" };
//args = new[] { "airports" };
//args = new[] { "simbrief" };
//args = new[] { "toolkit" };
//args = new[] { "jekyll" };
//args = new[] { "jekyll", "-d", "PADK", "-a", "PASY" };
//args = new[] { "trim" };
//args = new[] { "gps" };
//args = new[] { "rename", "-d", "PASY", "-a", "UHPP" };
//args = new[] { "preview", "-h", "300", "-d", "PASY", "-a", "UHPP" };
//args = new[] { "stats" };
//args = new[] { "crop" };
//args = new[] { "preview", "-w", "400", "-h", "300" };
//args = new[] { "geotag", "-d", "PADK", "-a", "PASY" };
//args = new[] { "event", "-d", "PASY", "-a", "UHPP" };
//args = new[] { "gps", "-d", "PADK", "-a", "PASY" };
//args = new[] { "compress", "-d", "PACD", "-a", "PASN" };
//args = new[] { "optimize", "-d", "PACD", "-a", "PASN" };
//args = new[] { "gps", "-d", "PACD", "-a", "PASN" };
//args = new[] { "event", "-d", "PACD", "-a", "PASN" };
//args = new[] { "crop", "-d", "PAPG", "-a", "PAYA" };

try
{
    Run();
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

void Run() {
    var config = new HardcodedConfig();
    var optionTypes = GetTypesWithVerbAttribute().ToArray();

    Parser.Default.ParseArguments(args, optionTypes)
        .WithParsed<AirportFileGeneratorOptions>(options => { new AirportFileGeneratorCommand(config).Run(options); })
        .WithParsed<RecorderOptions>(options => { new RecorderCommand(config).Run(options); })
        .WithParsed<SimbriefOptions>(options => { new SimbriefCommand(config).Run(options); })
        .WithParsed<WizardOptions>(options => { new WizardCommand(config).Run(options); })
        .WithParsed<ScanOptions>(options => { new ScanCommand(config).Run(options); })
        .WithParsed<ToolkitOptions>(options => { new ToolkitCommand(config).Run(options); })
        .WithParsed<TrimOptions>(options => { new TrimCommand(config).Run(options); })
        .WithParsed<RenameOptions>(options => { new RenameCommand(config).Run(options); })
        .WithParsed<StatsOptions>(options => { new StatsCommand(config).Run(options); })
        .WithParsed<PreviewOptions>(options => { new PreviewCommand(config).Run(options); })
        .WithParsed<ChartOptions>(options => { new ChartCommand(config).Run(options); })
        .WithParsed<NextOptions>(options => { new NextCommand(config).Run(); })
        .WithParsed<GeoTagOptions>(options => { new GeoTagCommand(config).Run(options); })
        .WithParsed<CompressOptions>(options => { new CompressCommand(config).Run(options); })
        .WithParsed<OptimizeOptions>(options => { new OptimizeCommand(config).Run(options); })
        .WithParsed<GpsOptions>(options => { new GpsCommand(config).Run(options); })
        .WithParsed<EventOptions>(options => { new EventCommand(config).Run(options); })
        .WithParsed<DirOptions>(options => { new DirCommand(config).Run(options); })
        .WithParsed<DispatchOptions>(options => { new DispatchCommand(config).Run(options); })
        .WithParsed<NaviOptions>(options => { new NaviCommand(config).Run(options); })
        .WithParsed<PhotoOptions>(options => { new PhotoCommand(config).Run(options); })
        .WithParsed<JekyllOptions>(options => { new JekyllCommand(config).Run(options); })
        .WithParsed<CropOptions>(options => { new CropCommand(config).Run(options); })
        .WithNotParsed(_ => Console.Write(string.Empty));
}

static IEnumerable<Type> GetTypesWithVerbAttribute()
{
    foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        if (type.GetCustomAttributes(typeof(VerbAttribute), true).Length > 0)
            yield return type;
}