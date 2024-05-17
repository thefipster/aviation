using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain;
using Microsoft.VisualBasic.FileIO;

namespace TheFipster.Aviation.Modules.OurAirports.Components
{
    public class OurAirportsImporter
    {
        public List<OurAirport> Filter(string importPath, int minRunwayLengthMeters, string[] excludedSurfaces, bool excludeClosed)
        {
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
                if (excludeClosed && airport.Type == "closed")
                    return;

                var runs = runways.Where(x =>
                    x.AirportRef == airport.Id
                    && x.LengthFeet.HasValue 
                    && x.LengthFeet > UnitConverter.MetersToFeet(minRunwayLengthMeters));

                foreach (var surface in excludedSurfaces)
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

            return selection;
        }

        private List<OurRegion> readRegions(string csvFile)
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

        private List<OurCountry> readCountries(string csvFile)
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

        private List<OurAirport> readAirports(string airportCsv)
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

        private List<OurRunway> readRunway(string csvFile)
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
