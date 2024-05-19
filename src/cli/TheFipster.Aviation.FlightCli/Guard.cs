using TheFipster.Aviation.Domain.Exceptions;

namespace TheFipster.Aviation.FlightCli
{
    public static class Guard
    {
        public static void EnsureConfig(IConfig config)
        {
            if (config == null)
                throw new MissingConfigException("Config is null.");
        }
    }
}
