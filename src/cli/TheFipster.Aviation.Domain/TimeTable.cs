
namespace TheFipster.Aviation.Domain
{
    public class TimeTable
    {
        public TimeTable()
        {
            
        }

        public TimeTable(long unixEpochSecs, string name)
        {
            Timestamp = unixEpochSecs;
            Name = name;
        }

        public TimeTable(DateTime dateTime, string name)
        {
            Timestamp = new DateTimeOffset(dateTime).ToUnixTimeSeconds();
            Name = name;
        }

        public TimeTable(string unixSecsAsString, string name)
        {
            Timestamp = long.Parse(unixSecsAsString);
            Name = name;
        }

        public long Timestamp { get; set; }
        public string Name { get; set; }
    }
}
