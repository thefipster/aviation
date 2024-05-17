using System.CodeDom;
using System.Reflection;
using CommandLine;
using TheFipster.Aviation.FlightCli;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Commands;
using TheFipster.Aviation.FlightCli.Options;

//args = new[] { "cmd" };
//   , "-d", "EDDL", "-a", "EDDL"
//   , "-i", "E:\\aviation\\Data\\OurAirports\\import", "-o", "E:\\aviation\\Data\\OurAirports\\export"
//   , "-w", "400", "-h", "300"

args = new[] { "wizard" };

try
{
    Run();
    Console.WriteLine();
    Console.WriteLine("Finished");
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

void Run()
{
    var config = new HardcodedConfig();
    executeCommand(args, config);
}

static IEnumerable<Type> findTypesByAttribute<T>() where T : Attribute
{
    foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        if (type.GetCustomAttribute<T>(true) != null)
            yield return type;
}

static IEnumerable<Type> findTypesByInheritance<T>() where T : class
{
    foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        if (typeof(T).IsAssignableFrom(type))
            yield return type;
}

static void executeCommand(string[] args, HardcodedConfig config)
{
    var optionTypes = findTypesByAttribute<VerbAttribute>().ToArray();
    Parser.Default.ParseArguments(args, optionTypes)
        .WithParsed<AirportFileGeneratorOptions>(options => { new AirportFileGeneratorCommand().Run(options, config); })
        .WithParsed<RecorderOptions>(options => { new RecorderCommand().Run(options, config); })
        .WithParsed<SimbriefOptions>(options => { new SimbriefCommand().Run(options, config); })
        .WithParsed<WizardOptions>(options => { new WizardCommand().Run(options, config); })
        .WithParsed<ScanOptions>(options => { new ScanCommand().Run(options, config); })
        .WithParsed<ToolkitOptions>(options => { new ToolkitCommand().Run(options, config); })
        .WithParsed<TrimOptions>(options => { new TrimCommand().Run(options, config); })
        .WithParsed<RenameOptions>(options => { new RenameCommand().Run(options, config); })
        .WithParsed<StatsOptions>(options => { new StatsCommand().Run(options, config); })
        .WithParsed<PreviewOptions>(options => { new PreviewCommand().Run(options, config); })
        .WithParsed<ChartOptions>(options => { new ChartCommand().Run(options, config); })
        .WithParsed<NextOptions>(options => { new NextCommand().Run(options, config); })
        .WithParsed<GeoTagOptions>(options => { new GeoTagCommand().Run(options, config); })
        .WithParsed<CompressOptions>(options => { new CompressCommand().Run(options, config); })
        .WithParsed<OptimizeOptions>(options => { new OptimizeCommand().Run(options, config); })
        .WithParsed<GpsOptions>(options => { new GpsCommand().Run(options, config); })
        .WithParsed<BlackBoxStatsOptions>(options => { new BlackBoxStatsCommand().Run(options, config); })
        .WithParsed<FlightDirCreateOptions>(options => { new FlightDirCreateCommand().Run(options, config); })
        .WithParsed<SimbriefDispatchMoveOptions>(options => { new SimbriefDispatchMoveCommand().Run(options, config); })
        .WithParsed<NaviOptions>(options => { new NaviCommand().Run(options, config); })
        .WithParsed<PhotoOptions>(options => { new PhotoCommand().Run(options, config); })
        .WithParsed<JekyllOptions>(options => { new JekyllCommand().Run(options, config); })
        .WithParsed<CropOptions>(options => { new CropCommand().Run(options, config); })
        .WithParsed<OurAirportsFilterOptions>(options => { new OurAirportsFilterCommand().Run(options, config); })
        .WithNotParsed(_ => Console.Write(string.Empty));
}
