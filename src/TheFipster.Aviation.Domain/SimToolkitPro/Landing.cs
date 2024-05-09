namespace TheFipster.Aviation.Domain.SimToolkitPro
{
    public class Landing : JsonBase
    {
        public string? LocalId { get; set; }

        public string? FleetId { get; set; }

        public string? FlightId { get; set; }

        public string? AircraftName { get; set; }

        public string? TouchdownSpeed { get; set; }

        public string? TouchdownVerticalSpeed { get; set; }

        public string? TouchdownYaw { get; set; }

        public string? TouchdownPitch { get; set; }

        public string? TouchdownRoll { get; set; }

        public string? TouchdownLatitude { get; set; }

        public string? TouchdownLongitude { get; set; }

        public string? TouchdownGforce { get; set; }

        public string? TouchdownHeading { get; set; }

        public string? DetectedRunway { get; set; }

        public string? DetectedAirfield { get; set; }

        public string? Departure { get; set; }

        public string? Arrival { get; set; }

        public string? Lastupdated { get; set; }
    }
}
