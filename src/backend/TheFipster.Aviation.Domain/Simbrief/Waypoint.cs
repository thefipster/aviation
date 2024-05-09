namespace TheFipster.Aviation.Domain.Simbrief
{
    public class Waypoint
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Airway { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
