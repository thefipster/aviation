using System.Text.Json;
using TheFipster.Aviation.CoreCli.Abstractions;

namespace TheFipster.Aviation.CoreCli
{
    public class JsonReader<T> : IJsonReader<T>
    {
        public T FromFile(string filepath)
        {
            if (!File.Exists(filepath))
                throw new ApplicationException($"File {filepath} was not found.");

            var json = File.ReadAllText(filepath);

            try
            {
                return FromText(json);
            }
            catch (ApplicationException ex)
            {
                throw new ApplicationException($"File {filepath} doesn't contain valid json.", ex);
            }
            
        }

        public T FromText(string json)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                T? result = JsonSerializer.Deserialize<T>(json, options);
                if (result == null)
                    throw new ApplicationException($"This is not correctly typed json.");

                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"This is not valid json. {ex.Message}", ex);
            }
        }
    }
}
