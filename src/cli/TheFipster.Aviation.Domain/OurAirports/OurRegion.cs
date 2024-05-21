namespace TheFipster.Aviation.Domain.OurAirports
{
    public class OurRegion : IOurAirportData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string LocalCode { get; set; }
        public string Name { get; set; }
        public string Continent { get; set; }
        public string IsoCountryCode { get; set; }
        public string WikipediaUri { get; set; }
        public string Keywords { get; set; }

        public static OurRegion FromOurAirportsCsv(string[] fields)
        {
            var region = new OurRegion();

            region.Id = int.Parse(fields[0]);
            region.Code = fields[1];
            region.LocalCode = fields[2];
            region.Name = fields[3];
            region.Continent = fields[4];
            region.IsoCountryCode = fields[5];
            region.WikipediaUri = fields[6];
            region.Keywords = fields[7];

            return region;
        }
    }
}
