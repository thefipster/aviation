using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Simbrief.Kml;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public class CreateNavigraphFlightCommand : ICommand<CreateNavigraphFlightOptions, Leg>
    {
        private readonly FileOperations fileOperations;
        private readonly XmlReader xmlReader;
        private readonly JsonReader<SimbriefKmlRaw> planReander;
        private IConfig config;

        public CreateNavigraphFlightCommand()
        {
            fileOperations = new FileOperations();
            xmlReader = new XmlReader();
            planReander = new JsonReader<SimbriefKmlRaw>();
        }

        public Leg Run(CreateNavigraphFlightOptions options, IConfig config)
        {
            Console.WriteLine(CreateNavigraphFlightOptions.Welcome);
            this.config = Guard.EnsureConfig(config);

            var flightPlanFile = ensureSingleFlightPlan();
            Console.WriteLine();
            Console.WriteLine("Your chosen flight plan will be: " + flightPlanFile);

            var leg = createLeg(flightPlanFile);
            var flightPath = fileOperations.CreateFlightFolder(config.FlightsFolder, leg);
            var newFlightPlan = Path.Combine(flightPath, $"{leg.From} - {leg.To} - Route.kml");

            File.Move(flightPlanFile, newFlightPlan);
            return leg;
        }

        private Leg createLeg(string flightPlanFile)
        {
            var planJson = xmlReader.ReadToJson(flightPlanFile);
            var plan = planReander.FromText(planJson);

            var flightNo = Directory.GetDirectories(config.FlightsFolder).Count() + 1;
            var departure = plan?.Kml?.Document?.Placemark?.FirstOrDefault(x => x.Point != null)?.Name?.ToUpper();
            var arrival = plan?.Kml?.Document?.Placemark?.LastOrDefault(x => x.Point != null)?.Name?.ToUpper();

            if (string.IsNullOrWhiteSpace(departure) || string.IsNullOrWhiteSpace(arrival))
                throw new ApplicationException("Departure and/or Arrival couldn't be determined.");

            return new Leg(flightNo, departure, arrival);
        }

        private string ensureSingleFlightPlan()
        {
            IEnumerable<string> files = Directory.GetFiles(config.NavigraphFolder, "*.kml");

            if (files.Count() == 0)
                files = scanForFlightPlan();

            if (files.Count() == 1)
                return files.First();

            Console.WriteLine();
            return StdOut.Choser(files);
        }

        private IEnumerable<string> scanForFlightPlan()
        {
            Console.WriteLine();
            Console.WriteLine($"There are currently no planned flights, please create a new flight plan at {config.NavigraphFolder}.");
            var files = fileOperations.ScanForFiles(config.NavigraphFolder, "*.kml");
            return files;
        }
    }
}
