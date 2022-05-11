using System.Text.Json;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.BlackBox
{
    public class JsonWriter
    {
        public void Write(BlackBoxFlight flight)
        {
            var location = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var dir = "Aviation";
            var file = $"{flight.Origin} - {flight.Destination} - BlackBox.json";

            var path = Path.Combine(location, dir, file);

            var json = JsonSerializer.Serialize(flight);
            File.WriteAllText(path, json);
        }
    }
}
