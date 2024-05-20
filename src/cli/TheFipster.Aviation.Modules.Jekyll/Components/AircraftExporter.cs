using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Modules.Jekyll.Model;
using TheFipster.Aviation.Modules.Airports.Components;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.Domain.Simbrief;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    internal class AircraftExporter
    {
        internal Model.Aircraft FromCombinedFlights(string flightsFolder, string airportFile)
        {
            var folders = Directory.GetDirectories(flightsFolder);

            var totalStats = new Stats();
            OurAirport lastAirport = null;
            Coordinate lastPosition = null;
            var visitedCountries = new List<string>();
            var airports = new OurAirportFinder(new JsonReader<IEnumerable<OurAirport>>(), airportFile);

            foreach (var folder in folders)
            {
                var departureIcao = new FlightMeta().GetDeparture(folder);
                var departure = airports.SearchWithIcao(departureIcao);
                if (!visitedCountries.Contains(departure.IsoCountryCode))
                    visitedCountries.Add(departure.IsoCountryCode);

                var arrivalIcao = new FlightMeta().GetArrival(folder);
                var arrival = airports.SearchWithIcao(arrivalIcao);
                if (!visitedCountries.Contains(arrival.IsoCountryCode))
                    visitedCountries.Add(arrival.IsoCountryCode);

                var flightFile = new FlightFileScanner().GetFile(folder, FileTypes.FlightJson);
                var flight = new JsonReader<FlightImport>().FromFile(flightFile);

                if (flight.HasStats)
                    processStats(totalStats, flight.Stats);

                if (flight.HasTrack)
                    lastPosition = flight.Track.Last();


                lastAirport = arrival;
            }

            var aircraft = new Model.Aircraft(folders.Count(), totalStats, lastAirport, lastPosition, visitedCountries);
            return aircraft;
        }

        private void processStats(Stats total, Stats stats)
        {
            total.FuelUsed += stats.FuelUsed;
            total.FlightTime += stats.FlightTime;
            total.Passengers += stats.Passengers;
            total.TrackDistance += stats.TrackDistance;
            total.TaxiDistance += stats.TaxiDistance;
            total.FlownDistance += stats.FlownDistance;
            total.RouteDistance += stats.RouteDistance;
            total.GreatCircleDistance += stats.GreatCircleDistance;

            if (stats.MaxGroundspeedMps > total.MaxGroundspeedMps)
                total.MaxGroundspeedMps = stats.MaxGroundspeedMps;

            if (stats.MaxAltitudeM > total.MaxAltitudeM)
                total.MaxAltitudeM = stats.MaxAltitudeM;
        }
    }
}
