using System.Text.Json;

namespace TheFipster.Aviation.CoreCli
{
    public class JsonWriter<T>
    {


        public void Write(T data, string fileType, string from, string? to = null)
        {
            var location = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            var file = string.IsNullOrEmpty(to)
                ? $"{@from} - {fileType}.json"
                : $"{@from} - {to} - {fileType}.json";

            var path = Path.Combine(location, "Aviation", "output");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, file);

            var json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }

        public void Write(string flightPath, T data, string fileType, string from, string? to = null)
        {
            var file = string.IsNullOrEmpty(to)
                ? $"{@from} - {fileType}.json"
                : $"{@from} - {to} - {fileType}.json";

            var path = Path.Combine(flightPath, file);

            var json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }
    }
}
