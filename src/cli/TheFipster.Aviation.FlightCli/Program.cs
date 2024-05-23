using System.Reflection;
using CommandLine;
using TheFipster.Aviation.FlightCli;
using TheFipster.Aviation.FlightCli.Commands;
using TheFipster.Aviation.FlightCli.Options;

//args = new[] { "cmd" };
//   , "-d", "EDDL", "-a", "EDDL"
//   , "-i", "E:\\aviation\\Data\\OurAirports\\import", "-o", "E:\\aviation\\Data\\OurAirports\\export"
//   , "-w", "400", "-h", "300"

//args = new[] { "process", "-d", "UHSB", "-a", "RJEC" };

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
        .WithParsed<NewManualFlightOptions>(options => { new NewManualFlightCommand().Run(options, config); })
        .WithParsed<NewDispatchFlightOptions>(options => { new NewDispatchFlightCommand().Run(options, config); })

        .WithParsed<CreateNavigraphFlightOptions>(options => { new CreateNavigraphFlightCommand().Run(options, config); })
        .WithParsed<CreateSimbriefFlightOptions>(options => { new CreateSimbriefFlightCommand().Run(options, config); })

        .WithParsed<BlackboxRecorderOptions>(options => { new BlackboxRecorderCommand().Run(options, config); })

        .WithParsed<ImportCollectorOptions>(options => { new ImportCollectorCommand().Run(options, config); })
        .WithParsed<ImportCombinerOptions>(options => { new ImportCombinerCommand().Run(options, config); })
        .WithParsed<ImportProcessorOptions>(options => { new ImportProcessorCommand().Run(options, config); })

        .WithParsed<JekyllBuildOptions>(options => { new JekyllBuildCommand().Run(options, config); })

        .WithParsed<ScanFolderOptions>(options => { new ScanFolderCommand().Run(options, config); })
        .WithParsed<JekyllCreateOptions>(options => { new JekyllCreateCommand().Run(options, config); })
        .WithParsed<OurAirportsFilterOptions>(options => { new OurAirportsFilterCommand().Run(options, config); })

        .WithNotParsed(_ => Console.WriteLine());
}
