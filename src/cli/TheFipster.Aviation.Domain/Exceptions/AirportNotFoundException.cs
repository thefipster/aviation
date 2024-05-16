namespace TheFipster.Aviation.Domain.Exceptions
{
    public class AirportNotFoundException : Exception
    {
        public AirportNotFoundException(string icao) 
            : base($"The airport {icao} couldn't be found in the JSON file.") { }
    }
}
