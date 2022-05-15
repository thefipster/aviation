using Microsoft.Extensions.Configuration;

namespace TheFipster.Aviation.FlightCli
{
    internal class Configuration
    {
        internal IConfiguration Load()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            try
            {
                var config = builder.Build();
                return config;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Couldn't load appsettings.json.", ex);
            }
        }
    }
}
