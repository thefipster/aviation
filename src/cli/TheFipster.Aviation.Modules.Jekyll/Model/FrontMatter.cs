using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Modules.Airports.Components;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    internal class FrontMatter
    {
        public FrontMatter(FlightImport flight, OurAirportFinder airports)
        {
            Arrival = airports.SearchWithIcao(flight.Arrival);
            Departure = airports.SearchWithIcao(flight.Departure);
            Title = Departure.Ident + " - " + Arrival.Ident;
            Description = Departure.Name + " - " + Arrival.Name;

            if (flight.HasSimbriefXml)
            {
                var distanceNm = int.Parse(flight.SimbriefXml.Ofp.General.RouteDistance);
                DistanceKm = UnitConverter.NauticalMilesToKilometers(distanceNm);
                Route = flight.SimbriefXml.Ofp.General.Route;
                AiracCycle = flight.SimbriefXml.Ofp.Params.Airac;
                var dispatchDate = long.Parse(flight.SimbriefXml.Ofp.Params.TimeGenerated);
                DispatchDate = dispatchDate;
                FlightNumber = $"FIP {flight.SimbriefXml.Ofp.General.FlightNumber}";
                LegNo = int.Parse(flight.SimbriefXml.Ofp.General.FlightNumber ?? "0");
            }
        }

        public string Layout { get; private set; } = "post";
        public string Title { get; set; }
        public string Description { get; set; }
        public string? AiracCycle { get; set; }
        public long DispatchDate { get; set; }
        public double DistanceKm { get; private set; }
        public OurAirport Arrival { get; private set; }
        public OurAirport Departure { get; private set; }
        public string FlightNumber { get; set; }
        public int LegNo { get; set; }
        public string Route { get; set; }
    }
}
