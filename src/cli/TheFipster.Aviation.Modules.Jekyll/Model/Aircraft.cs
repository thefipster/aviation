using TheFipster.Aviation.Domain.OurAirports;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    internal class Aircraft
    {
        public string Layout => "aircraft";
        public string Title => "Aircraft";
        public string Registration => "D-FIPS";
        public string Description => "Airbus A320neo 251N LEAP-1A";
        public string Image => "assets/images/aircraft.jpg";
        public bool Navmenu => true;
        public string Navtitle => "Aircraft";
        public int Navorder => 4;
        public string Icao { get; internal set; }
        public string Airport { get; internal set; }
        public string Country { get; internal set; }
        public string Region { get; internal set; }
        public string Continent { get; internal set; }
        public int Flights { get; set; }
        public double DistanceFlownKm { get; set; }
        public int SecondsFlown { get; internal set; }
        public int HighestAltitudeM { get; set; }
        public int HighestSpeedMps { get; set; }
        public int OverspeedBelow10000 { get; internal set; }
        public int FuelConsumedKg { get; internal set; }
        public int Passengers { get; internal set; }
        public decimal Latitude { get; internal set; }
        public decimal Longitude { get; internal set; }
        public Location Parking { get; set; }
    }
}
