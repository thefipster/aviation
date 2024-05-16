namespace TheFipster.Aviation.Domain.Simbrief
{
    public  class Notam
    {
        public string Id { get; set; }
        public string Icao { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Report { get; set; }
        public string Account { get; set; }
        public string Source { get; set; }
    }
}
