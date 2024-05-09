namespace TheFipster.Aviation.Domain.Simbrief
{
    public class Route : JsonBase
    {
        public Route()
        {
            Coordinates = new List<Coordinate>();
        }

        public Route(string departure, string arrival)
            : this()
        {
            Departure = departure;
            Arrival = arrival;
        }

        public string? Departure { get; set; }
        public string? Arrival { get; set; }
        public ICollection<Coordinate> Coordinates { get; set; }
    }
}
