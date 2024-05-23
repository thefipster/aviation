﻿using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.CoreCli.Extensions
{
    public static class FlightImportExtensions
    {
        public static string GetDeparture(this FlightImport flight)
        {
            if (flight.ActualDeparture != null)
                return flight.ActualDeparture.Airport;

            return flight.Departure;
        }

        public static string GetArrival(this FlightImport flight)
        {
            if (flight.ActualArrival != null)
                return flight.ActualArrival.Airport;

            return flight.Arrival;
        }

        public static string GetRoute(this FlightImport flight)
        {
            string? route = null;

            if (flight.HasSimbriefXml)
                route = flight.SimbriefXml.Ofp.General.Route;
            else if (flight.HasSimToolkitPro)
                route = flight.SimToolkitPro.Logbook.Route;

            var departure = flight.GetDeparture();
            var arrival = flight.GetArrival();

            if (string.IsNullOrWhiteSpace(route))
                return departure + " DCT " + arrival;

            if (!route.Contains(departure))
                route = departure + " " + route;

            if (!route.Contains(arrival))
                route = route + " " + arrival;

            return route;
        }
    }
}
