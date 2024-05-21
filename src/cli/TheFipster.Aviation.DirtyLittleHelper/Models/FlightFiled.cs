namespace TheFipster.Aviation.DirtyLittleHelper.Models
{
    public class FlightFiled
    {
        public FlightFiled(string[] line)
        {
            Departure = line[0];
            Arrival = line[1];
            Day = int.Parse(line[2]);
            Month = int.Parse(line[3]);
            Year = int.Parse(line[4]);
            Hour = int.Parse(line[5]);
            Minute = int.Parse(line[6]);
        }

        public string Departure { get; set; }
        public string Arrival { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public int Hour { get; set; }
        public int Minute { get; set; }

        public DateTime Date => new DateTime(Year, Month, Day, Hour, Minute, 0);
    }
}
