using System.Text.Json;

namespace TheFipster.Aviation.CoreCli
{
    public class JsonWriter<T>
    {
        public void Write(T data, string fileType, string from, string? to = null)
        {
            var location = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            Write(location, data, fileType, from, to);
        }

        public void Write(string flightPath, T data, string fileType, string from, string? to = null)
        {
            var file = string.IsNullOrEmpty(to)
                ? $"{@from} - {fileType}.json"
                : $"{@from} - {to} - {fileType}.json";

            Write(flightPath, data, file);
        }

        public void Write(string flightPath, T data, string filename, bool overwrite = false)
        {
            var path = Path.Combine(flightPath, filename);

            if (File.Exists(path) && !overwrite)
                return;

            var json = JsonSerializer.Serialize(data);
            File.WriteAllText(path, json);
        }
    }
}
