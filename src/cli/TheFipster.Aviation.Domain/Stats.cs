using TheFipster.Aviation.Domain.SimToolkitPro;

namespace TheFipster.Aviation.Domain
{
    public class Stats : JsonBase
    {
        public int FuelRamp { get; set; }
        public int FuelShutdown { get; set; }
        public int FuelUsed { get; set; }
        public string? Departure { get; set; }
        public string? Arrival { get; set; }
        public string? Route { get; set; }
        public int DepartureAt { get; set; }
        public int ArrivalAt { get; set; }
        public int FlightTime { get; set; }
        public string? AiracCycle { get; set; }
        public int DispatchAt { get; set; }
        public int Altitude { get; set; }
        public int WindComponent { get; set; }
        public int GreatCircleDistance { get; set; }
        public int RouteDistance { get; set; }
        public int FuelPlanned { get; set; }
        public int PrepTime { get; set; }
        public int FuelDelta { get; set; }
        public LandingStats? Landing { get; set; }
        public int TasPlanned { get; set; }
        public int Passengers { get; set; }
        public string? Remarks { get; set; }
        public int MaxWindspeedDirection { get; set; }
        public double MaxWindspeedMps { get; set; }
        public int MaxDescentMps { get; set; }
        public int MaxClimbMps { get; set; }
        public int MaxGroundspeedMps { get; set; }
        public int MaxAltitudeM { get; set; }
        public bool HasLogbook { get; set; }
        public bool HasSimbrief { get; set; }
        public bool HasLanding { get; set; }
        public bool HasBlackbox { get; set; }
        public double TrackDistance { get; set; }
        public bool Below10000SpeedWarning { get; set; }
        public double FlownDistance { get; set; }
        public double TaxiDistance { get; set; }

        public void Merge(BlackBoxStats extrems)
        {
            Below10000SpeedWarning = extrems.Below10000SpeedWarning;
            FlownDistance = extrems.DistanceAir;
            TaxiDistance = extrems.DistanceGround;
            MaxAltitudeM = extrems.MaxAltitudeM;
            MaxClimbMps = extrems.MaxClimbMps;
            MaxDescentMps = extrems.MaxDescentMps;
            MaxGroundspeedMps = extrems.MaxGroundSpeedMps;
            MaxWindspeedMps = extrems.MaxWindspeedMps;
            MaxWindspeedDirection = extrems.WindDirectionRad;
        }

        public void Merge(LogbookStats stats)
        {
            FlightTime = stats.FlightTime;
        }
    }
}
