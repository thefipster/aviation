namespace TheFipster.Aviation.Domain.SimToolkitPro
{
    public class Logbook : JsonBase
    {
        public string? LocalId { get; set; }

        public string? FleetId { get; set; }

        public string? FleetReg { get; set; }

        public string? FlightCallsign { get; set; }

        public string? FlightNumber { get; set; }

        public string? Dep { get; set; }

        public string? Arr { get; set; }

        public string? LandedAt { get; set; }

        public string? DidDivert { get; set; }

        public string? ActualDep { get; set; }

        public string? ActualArr { get; set; }

        public string? PausedSeconds { get; set; }

        public string? FuelRamp { get; set; }

        public string? FuelShutdown { get; set; }

        public string? Route { get; set; }

        public string? DocsRmk { get; set; }

        public string? TrackedGeoJson { get; set; }

        public string? Lastupdated { get; set; }
    }
}
