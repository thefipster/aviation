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
            PlannedDeparture = airports.SearchWithIcao(flight.Departure);
            PlannedArrival = airports.SearchWithIcao(flight.Arrival);

            if (flight.ActualArrival != null)
                Arrival = airports.SearchWithIcao(flight.ActualArrival.Airport);
            else
                Arrival = airports.SearchWithIcao(flight.Arrival);


            if (flight.ActualDeparture != null)
                Departure = airports.SearchWithIcao(flight.ActualDeparture.Airport);
            else
                Departure = airports.SearchWithIcao(flight.Departure);

            Title = Departure.Ident + " - " + Arrival.Ident;
            Description = Departure.Name + " - " + Arrival.Name;

            DistanceKm = flight.Stats.TrackDistance;
            RouteKm = flight.Stats.RouteDistance;
            GreatCircleKm = flight.Stats.GreatCircleDistance;

            Route = flight.GetRoute();

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
            FuelBurned = flight.Stats.FuelUsed;
        }

        public string Layout => "post";
        public string Title { get; set; }
        public string Description { get; set; }
        public string? AiracCycle { get; set; }
        public long DispatchDate { get; set; }
        public double DistanceKm { get; set; }
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
