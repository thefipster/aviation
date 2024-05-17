namespace TheFipster.Aviation.FlightCli
{
    public class HardcodedConfig : IConfig
    {
        public string FlightsFolder => "E:\\aviation\\Worldtour\\Flights";

        public string AirportFile => "E:\\aviation\\Data\\airport-codes.json";
        public string OurAirportFile => "E:\\aviation\\Data\\OurAirports\\export\\ourairports.json";
        public string OurAirportMetaFile => "E:\\aviation\\Data\\OurAirports\\export\\airport-meta.json";
        public string FlightPlanFile => "E:\\aviation\\Worldtour\\FlightPlan.json";

        public string ScreenshotFolder => "C:\\Users\\felix\\Videos\\Captures";
        public string SimbriefFolder => "E:\\aviation\\Data\\Simbrief";
        public string NavigraphFolder => "E:\\aviation\\Data\\Navigraph";
        public string SimToolkitProDatabaseFile => "C:\\Users\\felix\\OneDrive\\Dokumente\\SimToolkitPro\\userdata.db";

        public string JekyllFolder => "C:\\Users\\felix\\Source\\aviation\\src\\flog";
    }
}
