namespace TheFipster.Aviation.Domain.Exceptions
{
    public class DuplicateFlightException : Exception
    {
        public DuplicateFlightException(string departure, string arrival) 
            : base($"The flight from {departure} to {arrival} already exists.") { }
    }
}
