using System.Reflection;
using CommandLine;
using TheFipster.Aviation.FlightCli;
using TheFipster.Aviation.FlightCli.Commands;
using TheFipster.Aviation.FlightCli.Options;

//args = new[] { "cmd" };
//   , "-d", "EDDL", "-a", "EDDL"
//   , "-i", "E:\\aviation\\Data\\OurAirports\\import", "-o", "E:\\aviation\\Data\\OurAirports\\export"
//   , "-w", "400", "-h", "300"

//args = new[] { "download", "-d", "UHPP", "-a", "UHSI" };

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
    IConfig config = new HardcodedConfig();
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

static void executeCommand(string[] args, IConfig config)
{
    var optionTypes = findTypesByAttribute<VerbAttribute>().ToArray();
    Parser.Default.ParseArguments(args, optionTypes)
        .WithParsed<FlightDirCreateOptions>(options => { new FlightDirCreateCommand().Run(options, config); })
        .WithParsed<SimbriefDispatchMoveOptions>(options => { new SimbriefDispatchMoveCommand().Run(options, config); })
        .WithParsed<AirportFileGeneratorOptions>(options => { new AirportFileGeneratorCommand().Run(options, config); })
        .WithParsed<BlackboxRecorderOptions>(options => { new BlackboxRecorderCommand().Run(options, config); })
        .WithParsed<MoveNavigraphChartsOptions>(options => { new MoveNavigraphChartsCommand().Run(options, config); })
        .WithParsed<MoveScreenshotsOptions>(options => { new MoveScreenshotsCommand().Run(options, config); })
        .WithParsed<ImportToolkitOptions>(options => { new ImportToolkitCommand().Run(options, config); })
        .WithParsed<RenameImportFilesOptions>(options => { new RenameImportFilesCommand().Run(options, config); })
        .WithParsed<CropScreenshotTitleOptions>(options => { new CropScreenshotTitleCommand().Run(options, config); })
        .WithParsed<CreatePreviewForImagesOptions>(options => { new CreatePreviewForImagesCommand().Run(options, config); })
        .WithParsed<ConvertChartsToImageOptions>(options => { new ConvertChartsToImageCommand().Run(options, config); })
        .WithParsed<SimbriefDownloadOptions>(options => { new SimbriefDownloadCommand().Run(options, config); })

        .WithParsed<CombineImportsOptions>(options => { new CombineImportsCommand().Run(options, config); })
        .WithParsed<ProcessImportsOptions>(options => { new ProcessImportsCommand().Run(options, config); })

        .WithParsed<GeoTagScreenshotsOptions>(options => { new GeoTagScreenshotsCommand().Run(options, config); })
        .WithParsed<ProcessSimbriefOptions>(options => { new ProcessSimbriefCommand().Run(options, config); })
        .WithParsed<TrimBlackboxOptions>(options => { new TrimBlackboxCommand().Run(options, config); })
        .WithParsed<CompressBlackboxOptions>(options => { new CompressBlackboxCommand().Run(options, config); })
        .WithParsed<ProcessBlackboxOptions>(options => { new ProcessBlackboxCommand().Run(options, config); })
        .WithParsed<CombineGpsInformationOptions>(options => { new CombineGpsInformationCommand().Run(options, config); })
        .WithParsed<CombineStatsOptions>(options => { new CombineStatsCommand().Run(options, config); })
        .WithParsed<CompressTrackOptions>(options => { new CompressTrackCommand().Run(options, config); })

        .WithParsed<ScanOptions>(options => { new ScanCommand().Run(options, config); })
        .WithParsed<WizardOptions>(options => { new WizardCommand().Run(options, config); })
        .WithParsed<NextOptions>(options => { new NextCommand().Run(options, config); })
        .WithParsed<JekyllOptions>(options => { new JekyllCommand().Run(options, config); })
        .WithParsed<OurAirportsFilterOptions>(options => { new OurAirportsFilterCommand().Run(options, config); })
        .WithNotParsed(_ => Console.WriteLine());
}
