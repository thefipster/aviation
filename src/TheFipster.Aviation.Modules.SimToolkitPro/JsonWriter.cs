using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.SimToolkitPro
{
    public class JsonWriter
    {
        public void Write(SimToolkitProFlight flight)
        {
            var location = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var dir = "Aviation";
            var file = $"{flight.Logbook.Dep} - {flight.Logbook.Arr} - SimToolkitPro.json";

            var path = Path.Combine(location, dir, file);

            var json = JsonSerializer.Serialize(flight);
            File.WriteAllText(path, json);
        }
    }
}
