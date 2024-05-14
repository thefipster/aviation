namespace TheFipster.Aviation.Domain.Simbrief
{
    public class Waypoint
    {
        public Waypoint() { }

        public Waypoint(string name, double lat, double lon)
        {
            Name = name;
            Latitude = lat;
            Longitude = lon;
        }

        public Waypoint(int index, string name, double lat, double lon)
            : this(name, lat, lon)
        {
            Index = index;
        }

        public int Index { get; set; }
        public string Name { get; set; }
        public string Airway { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
