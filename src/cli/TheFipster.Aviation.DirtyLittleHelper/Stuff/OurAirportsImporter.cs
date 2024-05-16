using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain;
using Microsoft.VisualBasic.FileIO;

namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    internal class OurAirportsImporter
    {
        public void Do(string importPath, string exportPath)
        {
            int airportLimitPerFile = 20000;
            int runwayLimitMeters = 1500;
            string[] surfaceExclusion = ["WATER", "TURF", "GRASS", "DIRT", "WAT"];

            var airportFile = "airports.csv";
            var airportCsv = Path.Combine(importPath, airportFile);
            var airports = readAirports(airportCsv);

            var runwayFile = "runways.csv";
            var runwayCsv = Path.Combine(importPath, runwayFile);
            var runways = readRunway(runwayCsv);

            var countryFile = "countries.csv";
            var countryCsv = Path.Combine(importPath, countryFile);
            var countries = readCountries(countryCsv);

            var regionFile = "regions.csv";
            var regionCsv = Path.Combine(importPath, regionFile);
            var regions = readRegions(regionCsv);

            var selection = new List<OurAirport>();
            var continents = new Dictionary<string, List<OurAirport>>();

            var surfaces = new List<string>();

            Parallel.ForEach(airports, airport =>
            {
                if (airport.Type == "closed")
                    return;

                var runs = runways.Where(x =>
                    x.AirportRef == airport.Id
                    && x.LengthFeet.HasValue && x.LengthFeet > UnitConverter.MetersToFeet(runwayLimitMeters));

                foreach (var surface in surfaceExclusion)
                    runs = runs.Where(x => !x.Surface.ToUpper().Contains(surface));

                foreach (var surf in runs.Select(x => x.Surface))
                    if (!surfaces.Contains(surf.ToUpper()))
                        surfaces.Add(surf.ToUpper());

                if (!runs.Any())
                    return;

                var country = countries.FirstOrDefault(x => x.Code == airport.IsoCountryCode);
                var region = regions.FirstOrDefault(x => x.Code == airport.IsoRegionCode);

                airport.Runways = runs.ToList();
                airport.Country = country;
                airport.Region = region;

                selection.Add(airport);
            });

            if (selection.Count > airportLimitPerFile)
            {
                var buckets = Math.Ceiling((double)selection.Count / airportLimitPerFile);
                var softLimit = (int)Math.Ceiling(selection.Count / buckets);
                var list = selection.OrderBy(x => x.Longitude);
                int index = 0;
                var splitted = new Dictionary<int, List<OurAirport>>();
                while (index < list.Count())
                {
                    var key = splitted.Count() + 1;
                    splitted.Add(key, new List<OurAirport>());
                    splitted[key] = list.Skip(index).Take(softLimit).ToList();
                    index += softLimit;
                }

                foreach (var split in splitted)
                {
                    var splitFile = Path.Combine(exportPath, "airports-" + split.Key + ".json");
                    new JsonWriter<IEnumerable<OurAirport>>().Write(splitFile, split.Value, true);
                }
            }
            else
            {
                var file = Path.Combine(exportPath, "airports.json");
                new JsonWriter<IEnumerable<OurAirport>>().Write(file, selection, true);
            }
        }

        static List<OurRegion> readRegions(string csvFile)
        {
            using (TextFieldParser csvParser = new TextFieldParser(csvFile))
            {
                csvParser.CommentTokens = ["#"];
                csvParser.SetDelimiters([","]);
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip header
                csvParser.ReadLine();

                var list = new List<OurRegion>();
                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    var item = OurRegion.FromOurAirportsCsv(fields);
                    list.Add(item);
                }

                return list;
            }
        }

        static List<OurCountry> readCountries(string csvFile)
        {
            using (TextFieldParser csvParser = new TextFieldParser(csvFile))
            {
                csvParser.CommentTokens = ["#"];
                csvParser.SetDelimiters([","]);
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip header
                csvParser.ReadLine();

                var countries = new List<OurCountry>();
                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    var country = OurCountry.FromOurAirportsCsv(fields);
                    countries.Add(country);
                }

                return countries;
            }
        }

        static List<OurAirport> readAirports(string airportCsv)
        {
            using (TextFieldParser csvParser = new TextFieldParser(airportCsv))
            {
                csvParser.CommentTokens = ["#"];
                csvParser.SetDelimiters([","]);
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip header
                csvParser.ReadLine();

                var airports = new List<OurAirport>();
                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    var airport = OurAirport.FromOurAirportsCsv(fields);
                    airports.Add(airport);
                }

                return airports;
            }
        }

        static List<OurRunway> readRunway(string csvFile)
        {
            using (TextFieldParser csvParser = new TextFieldParser(csvFile))
            {
                csvParser.CommentTokens = ["#"];
                csvParser.SetDelimiters([","]);
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip header
                csvParser.ReadLine();

                var runways = new List<OurRunway>();
                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    var runway = OurRunway.FromOurAirportsCsv(fields);
                    runways.Add(runway);
                }

                return runways;
            }
        }
    }
}
