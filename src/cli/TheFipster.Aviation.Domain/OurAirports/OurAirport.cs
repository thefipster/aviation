using System.Globalization;

namespace TheFipster.Aviation.Domain.OurAirports
{
    public class OurAirport : IOurAirportData
    {
        public OurAirport()
        {
            Runways = new List<OurRunway>();
        }

        public int Id { get; set; }
        public string Ident { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? ElevationFeet { get; set; }
        public string ContinentCode { get; set; }
        public string IsoCountryCode { get; set; }
        public string IsoRegionCode { get; set; }
        public string Municipality { get; set; }
        public string ScheduledService { get; set; }
        public string GpsCode { get; set; }
        public string IataCode { get; set; }
        public string LocalCode { get; set; }
        public string WebUri { get; set; }
        public string WikipediaUri { get; set; }
        public string Keywords { get; set; }
        public ICollection<OurRunway> Runways { get; set; }
        public OurCountry Country { get; set; }
        public OurRegion Region { get; set; }

        public static OurAirport FromOurAirportsCsv(string[] fields)
            => new OurAirport
            {
                Id = int.Parse(fields[0]),
                Ident = fields[1],
                Type = fields[2],
                Name = fields[3],
                Latitude = double.Parse(fields[4], CultureInfo.InvariantCulture),
                Longitude = double.Parse(fields[5], CultureInfo.InvariantCulture),
                ElevationFeet = !string.IsNullOrEmpty(fields[6]) ? int.Parse(fields[6]) : null,
                ContinentCode = fields[7],
                IsoCountryCode = fields[8],
                IsoRegionCode = fields[9],
                Municipality = fields[10],
                ScheduledService = fields[11],
                GpsCode = fields[12],
                IataCode = fields[13],
                LocalCode = fields[14],
                WebUri = fields[15],
                WikipediaUri = fields[16],
                Keywords = fields[17]
            };
    }
}
