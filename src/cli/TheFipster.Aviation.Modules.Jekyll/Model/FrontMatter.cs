using TheFipster.Aviation.CoreCli.Extensions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Modules.Airports.Components;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    public class FrontMatter
    {
        public FrontMatter(FlightImport flight, OurAirportFinder airports)
        {
            PlannedDeparture = airports.SearchWithIcao(flight.GetDeparturePlannedIcao());
            Departure = airports.SearchWithIcao(flight.GetDepartureIcao());

            PlannedArrival = airports.SearchWithIcao(flight.GetArrivalPlannedIcao());
            Arrival = airports.SearchWithIcao(flight.GetArrivalcao());

            Route = flight.GetRouteTxt();
            FuelBurned = flight.GetFuelUsedKg();
            DistanceKm = flight.GetDistanceFlownKm();

            Title = Departure.Ident + " - " + Arrival.Ident;
            Description = Departure.Name + " - " + Arrival.Name;

            RouteKm = flight.Stats.RouteDistance;
            GreatCircleKm = flight.Stats.GreatCircleDistance;

            if (flight.Started.HasValue)
                DispatchDate = new DateTimeOffset(flight.Started.Value).ToUnixTimeSeconds();
            else if (flight.HasSimbriefXml)
                DispatchDate = long.Parse(flight.SimbriefXml.Ofp.Params.TimeGenerated);

            FlightNumber = $"FIP {int.Parse(flight.FlightNumber):d4}";
            LegNo = int.Parse(flight.FlightNumber);

            if (flight.HasSimbriefXml)
                AiracCycle = flight.SimbriefXml.Ofp.Params.Airac;

            FuelRamp = flight.Stats.FuelRamp;
            FuelShutdown = flight.Stats.FuelShutdown;
        }

        public string Layout => "post";
        public string Title { get; set; }
        public string Description { get; set; }
        public string? AiracCycle { get; set; }
        public long DispatchDate { get; set; }
        public int DistanceKm { get; set; }
        public int RouteKm { get; set; }
        public int GreatCircleKm { get; set; }
        public OurAirport Arrival { get; set; }
        public OurAirport Departure { get; set; }
        public string FlightNumber { get; set; }
        public int LegNo { get; set; }
        public string Route { get; set; }
        public OurAirport PlannedDeparture { get; set; }
        public OurAirport PlannedArrival { get; set; }
        public int FuelBurned { get; set; }
        public int FuelRamp { get; set; }
        public int FuelShutdown { get; set; }
    }
}
