namespace TheFipster.Aviation.Domain.Simbrief
{
    public class Waypoint
    {
        public Waypoint() { }

        public Waypoint(int index, string name, double lat, double lon)
        {
            Index = index;
            Name = name;
            Latitude = lat;
            Longitude = lon;
        }

        public int Index { get; set; }
        public string Name { get; set; }
        public string Airway { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
