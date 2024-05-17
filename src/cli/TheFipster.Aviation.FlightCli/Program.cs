using System.Reflection;
using CommandLine;
using TheFipster.Aviation.FlightCli;
using TheFipster.Aviation.FlightCli.Commands;
using TheFipster.Aviation.FlightCli.Options;

//args = new[] { "cmd" };
//   , "-d", "EDDL", "-a", "EDDL"
//   , "-i", "E:\\aviation\\Data\\OurAirports\\import", "-o", "E:\\aviation\\Data\\OurAirports\\export"
//   , "-w", "400", "-h", "300"

args = new[] { "ourairports", "-i", "E:\\aviation\\Data\\OurAirports\\import", "-o", "E:\\aviation\\Data\\OurAirports\\export" };

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
    var optionTypes = findOptionTypes().ToArray();
    executeCommand(args, config, optionTypes);
}

static IEnumerable<Type> findOptionTypes()
{
    foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
        if (type.GetCustomAttributes(typeof(VerbAttribute), true).Length > 0)
            yield return type;
}

static void executeCommand(string[] args, HardcodedConfig config, Type[] optionTypes)
{
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
        .WithParsed<FlightDirCreateOptions>(options => { new FlightDirCreateCommand(config).Run(options); })
        .WithParsed<SimbriefDispatchMoveOptions>(options => { new SimbriefDispatchMoveCommand(config).Run(options); })
        .WithParsed<NaviOptions>(options => { new NaviCommand(config).Run(options); })
        .WithParsed<PhotoOptions>(options => { new PhotoCommand(config).Run(options); })
        .WithParsed<JekyllOptions>(options => { new JekyllCommand(config).Run(options); })
        .WithParsed<CropOptions>(options => { new CropCommand(config).Run(options); })
        .WithParsed<OurAirportsFilterOptions>(options => { new OurAirportsFilterCommand().Run(options, config); })
        .WithNotParsed(_ => Console.Write(string.Empty));
}