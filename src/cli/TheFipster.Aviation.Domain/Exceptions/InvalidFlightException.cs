namespace TheFipster.Aviation.Domain.Exceptions
{
    public class InvalidFlightException : Exception
    {
        public InvalidFlightException(string departure, string arrival, string reason)
            : base($"Invalid flight {departure} - {arrival}: {reason}") { }
    }
}
