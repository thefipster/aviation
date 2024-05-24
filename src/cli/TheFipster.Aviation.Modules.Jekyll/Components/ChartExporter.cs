using System.Collections.Concurrent;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.CoreCli.Extensions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Modules.Jekyll.Model;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    public class ChartExporter
    {
        private readonly FlightFileScanner scanner;
        private readonly JsonReader<FlightImport> loader;

        public ChartExporter()
        {
            scanner = new FlightFileScanner();
            loader = new JsonReader<FlightImport>();
        }

        public FlightPlot<int> ExportFuelChart(string flightsFolder)
        {
            var folders = Directory.GetDirectories(flightsFolder);
            var fuelMap = new ConcurrentDictionary<int, (string, int)>();
            Parallel.ForEach(folders, folder =>
            {
                var flightFile = scanner.GetFile(folder, FileTypes.FlightJson);
                var flight = loader.FromFile(flightFile);

                var fuelUsed = flight.GetFuelUsedKg();
                var distance = flight.GetDistanceFlownKm();

                var name = flight.GetName();
                var milage = (int)Math.Round(fuelUsed / (distance / 100d), 0);

                while (!fuelMap.TryAdd(int.Parse(flight.FlightNumber), (name, milage)))
                {
                    Thread.Sleep(10);
                }
            });

            var plot = new FlightPlot<int>(
                fuelMap.OrderBy(x => x.Key).Select(x => x.Value.Item1),
                fuelMap.OrderBy(x => x.Key).Select(x => x.Value.Item2));

            return plot;
        }

        public FlightPlot<int> ExportSpeedChart(string flightsFolder)
        {
            var folders = Directory.GetDirectories(flightsFolder);
            var fuelMap = new ConcurrentDictionary<int, (string, int)>();
            Parallel.ForEach(folders, folder =>
            {
                var flightFile = scanner.GetFile(folder, FileTypes.FlightJson);
                var flight = loader.FromFile(flightFile);

                var maxGs = flight.GetMaxGroundSpeedKmh();
                var name = flight.GetName();

                while (!fuelMap.TryAdd(int.Parse(flight.FlightNumber), (name, maxGs)))
                {
                    Thread.Sleep(10);
                }
            });

            var plot = new FlightPlot<int>(
                fuelMap.OrderBy(x => x.Key).Select(x => x.Value.Item1),
                fuelMap.OrderBy(x => x.Key).Select(x => x.Value.Item2));

            return plot;
        }
    }
}