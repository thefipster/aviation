namespace TheFipster.Aviation.FlightCli
{
    public interface IConfig
    {
        string FlightsFolder { get; }

        string AirportFile { get; }
        string OurAirportFile { get; }
        string FlightPlanFile { get; }

        string ScreenshotFolder { get; }
        string SimbriefFolder { get; }
        string NavigraphFolder { get; }
        string SimToolkitProDatabaseFile { get; }

        string JekyllFolder { get; }
    }
}
