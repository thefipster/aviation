using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.CoreCli.Extensions
{
    public static class FlightImportExtensions
    {
        public static string GetDepartureIcao(this FlightImport flight)
        {
            if (flight.ActualDeparture != null)
                return flight.ActualDeparture.Airport;

            return flight.Departure;
        }

        public static string GetArrivalcao(this FlightImport flight)
        {
            if (flight.ActualArrival != null)
                return flight.ActualArrival.Airport;

            return flight.Arrival;
        }

        public static string GetName(this FlightImport flight)
            => $"{flight.GetDepartureIcao()} - {flight.GetArrivalcao()}";

        public static string GetArrivalPlannedIcao(this FlightImport flight)
            => flight.Arrival;

        public static string GetDeparturePlannedIcao(this FlightImport flight)
            => flight.Arrival;

        public static string GetRouteTxt(this FlightImport flight)
        {
            string? route = null;

            if (flight.HasSimbriefXml)
                route = flight.SimbriefXml.Ofp.General.Route;
            else if (flight.HasSimToolkitPro)
                route = flight.SimToolkitPro.Logbook.Route;

            var departure = flight.GetDepartureIcao();
            var arrival = flight.GetArrivalcao();

            if (string.IsNullOrWhiteSpace(route))
                return departure + " DCT " + arrival;

            if (!route.Contains(departure))
                route = departure + " " + route;

            if (!route.Contains(arrival))
                route = route + " " + arrival;

            return route;
        }

        public static int GetFuelUsedKg(this FlightImport flight)
        {
            double fuel = 0;
            if (flight.HasStats)
                fuel = flight.Stats.FuelUsed;

            if (fuel <= 0 && flight.HasSimbriefXml)
                fuel = int.Parse(flight.SimbriefXml.Ofp.Fuel.PlanRamp) - int.Parse(flight.SimbriefXml.Ofp.Fuel.PlanLanding);

            return (int)Math.Round(fuel, 0);
        }

        public static int GetDistanceFlownKm(this FlightImport flight)
            => (int)Math.Round(flight.Stats.TrackDistance, 0);

        public static int GetMaxGroundSpeedKmh(this FlightImport flight)
            => (int)Math.Round(UnitConverter.MpsToKmh(flight.Stats.MaxGroundspeedMps), 0);
    }
}
