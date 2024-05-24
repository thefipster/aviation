using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.CoreCli;
using System.Collections.Concurrent;

namespace TheFipster.Aviation.Modules.OurAirports.Components
{
    public class OurAirportsImporter
    {
        public const string AirportFile = "airports.csv";
        public const string RunwayFile = "runways.csv";
        public const string CountryFile = "countries.csv";
        public const string RegionFile = "regions.csv";

        private readonly CsvReader csvReader;

        public OurAirportsImporter()
        {
            csvReader = new CsvReader();
        }

        public List<OurAirport> Filter(string importPath, int minRunwayLengthMeters, string[] excludedSurfaces, bool excludeClosed)
        {
            var airports = readFile<OurAirport>(AirportFile, importPath);
            var runways = readFile<OurRunway>(RunwayFile, importPath);
            var countries = readFile<OurCountry>(CountryFile, importPath);
            var regions = readFile<OurRegion>(RegionFile, importPath);

            var dataset = merge(minRunwayLengthMeters, excludedSurfaces, excludeClosed, airports, runways, countries, regions);
            return dataset;
        }

        private List<T> readFile<T>(string filename, string path) where T : IOurAirportData
        {
            var csvFile = Path.Combine(path, filename);
            var lines = csvReader.FromFile(csvFile);
            var bag = new ConcurrentBag<T>();
            Parallel.ForEach(lines, line =>
            {
                var item = (T)new OurAirportFactory().Produce<T>(line);
                bag.Add(item);
            });
            return bag.ToList();
        }

        private static List<OurAirport> merge(int minRunwayLengthMeters, string[] excludedSurfaces, bool excludeClosed, List<OurAirport> airports, List<OurRunway> runways, List<OurCountry> countries, List<OurRegion> regions)
        {
            var selection = new ConcurrentBag<OurAirport>();
            Parallel.ForEach(airports, airport =>
            {
                if (excludeClosed && airport.Type == "closed")
                    return;

                var runs = runways.Where(x =>
                    x.AirportRef == airport.Id
                    && x.LengthFeet.HasValue
                    && x.LengthFeet > UnitConverter.MToFt(minRunwayLengthMeters));

                foreach (var surface in excludedSurfaces)
                    runs = runs.Where(x => !x.Surface.ToUpper().Contains(surface));

                if (!runs.Any())
                    return;

                var country = countries.FirstOrDefault(x => x.Code == airport.IsoCountryCode);
                var region = regions.FirstOrDefault(x => x.Code == airport.IsoRegionCode);

                airport.Runways = runs.ToList();
                airport.Country = country;
                airport.Region = region;

                selection.Add(airport);
            });

            return selection.ToList();
        }
    }
}
