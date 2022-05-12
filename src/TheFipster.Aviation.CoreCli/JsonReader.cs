using System.Text.Json;

namespace TheFipster.Aviation.CoreCli
{
    public class JsonReader<T>
    {
        public T Read(string filepath)
        {
            if (!File.Exists(filepath))
                throw new ApplicationException($"File {filepath} was not found.");

            var json = File.ReadAllText(filepath);

            try
            {
                var result = JsonSerializer.Deserialize<T>(json);
                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"File {filepath} is not valid json. {ex.Message}");
            }
        }
    }
}
