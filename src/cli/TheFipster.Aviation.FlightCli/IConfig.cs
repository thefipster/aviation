namespace TheFipster.Aviation.FlightCli
{
    public interface IConfig
    {
        string FlightsFolder { get; }
        string JekyllFolder { get; }

        string AirportFile { get; }
        string OurAirportFile { get; }
        string FlightPlanFile { get; }

        string SimbriefPilotId { get; }
        string SimbriefFolder { get; }

        string ScreenshotFolder { get; }
        string NavigraphFolder { get; }
        string SimToolkitProDatabaseFile { get; }

    }
}
