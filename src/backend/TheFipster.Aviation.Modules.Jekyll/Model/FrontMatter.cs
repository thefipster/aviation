using BAMCIS.GeoJSON;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Modules.Airports.Components;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    internal class FrontMatter
    {
        public FrontMatter(SimBriefFlight simbrief, OurAirportFinder airports)
        {
            Distance = UnitConverter.NauticalMilesToKilometers(simbrief.RouteDistance);
            ArrivalIcao = simbrief.Arrival.Icao;
            DepartureIcao = simbrief.Departure.Icao;
            DepartureName = simbrief.Departure.Name;
            Route = simbrief.Route ?? simbrief.Departure.Icao + "/" + simbrief.Departure.Runway + " DCT " + simbrief.Arrival.Icao + "/" + simbrief.Arrival.Runway;
            AiracCycle = simbrief.AiracCycle;
            DispatchDate = simbrief.DispatchDate;
            Description = simbrief.Departure.Name + " - " + simbrief.Arrival.Name;
            FlightNumber = $"FIP {simbrief.FlightNumber}";
            LegNo = int.Parse(simbrief.FlightNumber ?? "0");
            Title = simbrief.Departure.Icao + " - " + simbrief.Arrival.Icao;

            var arrivalAirport = airports.SearchWithIcao(ArrivalIcao);
            ArrivalContinent = Continents.Dictionary[arrivalAirport.ContinentCode];
            ArrivalCountry = arrivalAirport.Country.Name;
            ArrivalRegion = arrivalAirport.Region.Name;
            ArrivalName = arrivalAirport.Name.Replace("Airport", string.Empty);
            ArrivalCountryCode = arrivalAirport.IsoCountryCode;
            ArrivalContinentCode = arrivalAirport.ContinentCode;
            ArrivalRegionCode = arrivalAirport.IsoRegionCode;
        }

        public string Layout { get; private set; } = "post";
        public string Title { get; set; }
        public string Description { get; set; }
        public string DepartureIcao { get; set; }
        public string DepartureName { get; set; }
        public string? AiracCycle { get; set; }
        public long DispatchDate { get; set; }
        public double Distance { get; private set; }
        public string ArrivalIcao { get; set; }
        public string ArrivalName { get; set; }
        public string ArrivalCountryCode { get; private set; }
        public string ArrivalContinentCode { get; private set; }
        public string ArrivalRegionCode { get; private set; }
        public string FlightNumber { get; set; }
        public int LegNo { get; set; }
        public string Route { get; set; }
        public string ArrivalContinent { get; private set; }
        public string ArrivalCountry { get; private set; }
        public string ArrivalRegion { get; private set; }
    }
}
