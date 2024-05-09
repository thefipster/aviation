namespace TheFipster.Aviation.Domain
{
    public class Leg
    {
        public Leg(int no, string from, string to)
        {
            No = no;
            From = from;
            To = to;
        }

        public int No { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
