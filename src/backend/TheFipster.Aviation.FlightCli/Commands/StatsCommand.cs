using System.Globalization;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class StatsCommand
    {
        private HardcodedConfig config;

        public StatsCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(StatsOptions options)
        {
            Console.WriteLine("Scanning flight and outputing some stats.");
            IEnumerable<string> folders = Enumerable.Empty<string>();

            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];

            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");
                var stats = makeState(folder);
                new JsonWriter<Stats>().Write(folder, stats, Domain.Enums.FileTypes.StatsJson, stats.Departure, stats.Arrival, true);
            }
        }

        private Stats makeState(string folder)
        {
            var stats = new Stats();

            var meta = new FlightMeta();
            stats.Departure = meta.GetDeparture(folder);
            stats.Arrival = meta.GetArrival(folder);

            try
            {
                var logbookFile = new FlightFileScanner().GetFile(folder, Domain.Enums.FileTypes.LogbookJson);
                var logbook = new JsonReader<Logbook>().FromFile(logbookFile);

                stats.HasLogbook = true;
                stats.FuelRamp = int.Parse(logbook.FuelRamp);
                stats.FuelShutdown = int.Parse(logbook.FuelShutdown);
                stats.DepartureAt = int.Parse(logbook.ActualDep);
                stats.ArrivalAt = int.Parse(logbook.ActualArr);
                stats.Route = logbook.Route;
                stats.Remarks = logbook.DocsRmk;

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("\t\t no SimToolkitPro file");
            }

            try
            {
                var simbriefFile = new FlightFileScanner().GetFile(folder, Domain.Enums.FileTypes.SimbriefJson);
                var simbrief = new JsonReader<SimBriefFlight>().FromFile(simbriefFile);

                stats.HasSimbrief = true;
                stats.AiracCycle = simbrief.AiracCycle;
                stats.DispatchAt = unchecked((int)simbrief.DispatchDate);
                stats.Altitude = simbrief.Altitude;
                stats.WindComponent = simbrief.WindComponent;
                stats.GreatCircleDistance = simbrief.GreatCircleDistance;
                stats.RouteDistance = simbrief.RouteDistance;
                stats.FuelPlanned = simbrief.FuelBurn;
                stats.TasPlanned = simbrief.PlannedTas;
                stats.Passengers = simbrief.Passengers;
            }
            catch (FileNotFoundException)
            {

                Console.WriteLine("\t\t no Simbrief file");
            }

            try
            {
                var landingFile = new FlightFileScanner().GetFile(folder, Domain.Enums.FileTypes.LandingJson);
                var landing = new JsonReader<Landing>().FromFile(landingFile);

                stats.HasLanding = true;
                stats.Landing = new LandingStats();
                stats.Landing.Speed = int.Parse(landing.TouchdownSpeed);
                stats.Landing.VerticalSpeed = int.Parse(landing.TouchdownVerticalSpeed);
                stats.Landing.Gforce = double.Parse(landing.TouchdownGforce, CultureInfo.InvariantCulture);

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("\t\t no landing file");
            }

            try
            {
                var blackboxFile = new FlightFileScanner().GetFile(folder, Domain.Enums.FileTypes.BlackBoxStatsJson);
                var blackbox = new JsonReader<BlackBoxStats>().FromFile(blackboxFile);

                stats.HasBlackbox = true;
                stats.MaxAltitudeM = blackbox.MaxAltitudeM;
                stats.MaxGroundspeedMps = blackbox.MaxGroundSpeedMps;
                stats.MaxClimbMps = blackbox.MaxClimbMps;
                stats.MaxDescentMps = blackbox.MaxDescentMps;
                stats.MaxWindspeedMps = blackbox.MaxWindspeed;
                stats.MaxWindspeedDirection = blackbox.WindDirectionRad;

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("\t\t no Blackbox file");
            }

            if (stats.HasLogbook)
            {
                stats.FuelUsed = stats.FuelRamp - stats.FuelShutdown;
                stats.FlightTime = stats.ArrivalAt - stats.DepartureAt;
                stats.PrepTime = stats.DepartureAt - stats.DispatchAt;
            }

            if (stats.HasLogbook && stats.HasSimbrief)
            {
                stats.FuelDelta = stats.FuelUsed - stats.FuelPlanned;
            }

            return stats;
        }
    }
}
