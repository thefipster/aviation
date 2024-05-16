namespace TheFipster.Aviation.Domain.OurAirports
{
    public class OurCountry
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Continent { get; set; }
        public string WikipediaUri { get; set; }
        public string Keywords { get; set; }

        public static OurCountry FromOurAirportsCsv(string[] fields)
        {
            var country = new OurCountry();

            country.Id = int.Parse(fields[0]);
            country.Code = fields[1];
            country.Name = fields[2];
            country.Continent = fields[3];
            country.WikipediaUri = fields[4];
            country.Keywords = fields[5];

            return country;
        }
    }
}
