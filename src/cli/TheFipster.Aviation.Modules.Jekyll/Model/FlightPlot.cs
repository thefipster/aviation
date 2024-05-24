using System.Text.Json.Serialization;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    public class FlightPlot<T>
    {
        public FlightPlot(IEnumerable<string> flightNames, IEnumerable<T> values)
        {
            Flights = flightNames;
            Value = values;
        }

        [JsonPropertyName("x")]
        public IEnumerable<string> Flights { get; set; }

        [JsonPropertyName("y")]
        public IEnumerable<T> Value { get; set; }
    }
}
