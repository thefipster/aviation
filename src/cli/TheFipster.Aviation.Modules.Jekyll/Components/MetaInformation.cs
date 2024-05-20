﻿using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    internal class MetaInformation
    {
        public static string GeneratePostUrl(string flightNumber, string departure, string arrival)
            => $"{flightNumber}-{departure}-{arrival}.html";

        public static string GeneratePostUrl(SimBriefFlight simbrief)
            => GeneratePostName(simbrief.FlightNumber, simbrief.Departure.Icao, simbrief.Arrival.Icao);
        
        public static string GeneratePostName(string flightNumber, string departureIcao, string arrivalIcao)
            => $"{flightNumber}-{departureIcao}-{arrivalIcao}.html";

        public static string GeneratePostName(SimBriefFlight simbrief)
            => GeneratePostName(simbrief.FlightNumber, simbrief.Departure.Icao, simbrief.Arrival.Icao);

        public static string GeneratePostName(int flightNumber, string departureIcao, string arrivalIcao)
            => GeneratePostName(flightNumber.ToString(), departureIcao, arrivalIcao);

        internal static string? GenerateScreenshotUrl(string screenshot, int flightNumber)
            // EKVG - BIEG - Screenshot - 1.png
            // /assets/images/screenshots/38/PAHO%20-%20PAKN%20-%20Screenshot%20-%201.png
            => $"/assets/images/screenshots/{flightNumber}/{screenshot.Replace(" ", string.Empty)}.jpg";
    }
}
