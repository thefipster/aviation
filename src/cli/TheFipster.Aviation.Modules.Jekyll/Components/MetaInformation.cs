using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    internal class MetaInformation
    {
        public static string GeneratePostUrl(SimBriefFlight simbrief)
        {
            var flightDate = DateTime.UnixEpoch.AddSeconds(simbrief.DispatchDate);
            return $"{flightDate.Year}/{flightDate.Month:00}/{flightDate.Day:00}/{simbrief.FlightNumber}-{simbrief.Departure.Icao}-{simbrief.Arrival.Icao}.html";
        }

        public static string GeneratePostName(SimBriefFlight simbrief)
        {
            var flightDate = DateTime.UnixEpoch.AddSeconds(simbrief.DispatchDate);
            return $"{flightDate.Year}-{flightDate.Month}-{flightDate.Day}-{simbrief.FlightNumber}-{simbrief.Departure.Icao}-{simbrief.Arrival.Icao}.html";
        }

        internal static string? GenerateScreenshotUrl(string screenshot, int flightNumber)
        {
            // EKVG - BIEG - Screenshot - 1.png
            // /assets/images/screenshots/38/PAHO%20-%20PAKN%20-%20Screenshot%20-%201.png

            return $"/assets/images/screenshots/{flightNumber}/{screenshot.Replace(" ", string.Empty)}.jpg";
        }
    }
}
