using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Datahub;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.Merger;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class MergeCommand
    {
        internal void Run(MergeOptions options)
        {
            var simbrief = new JsonReader<SimBriefFlight>().FromFile(options.SimbriefFile);
            var blackbox = new JsonReader<BlackBoxFlight>().FromFile(options.BlackboxFile);
            var toolkit = new JsonReader<SimToolkitProFlight>().FromFile(options.ToolkitFile);
            var airports = new List<Airport>();
            foreach (var airportFile in options.AirportFiles)
            {
                var airport = new JsonReader<Airport>().FromFile(airportFile);
                airports.Add(airport);
            }

            var mergedAirports = new AirportMerger().Merge(airports, simbrief);
            var mergedLanding = new LandingMerger().Merge(toolkit);
            var mergedBlackBox = new BlackBoxMerger().Merge(blackbox);
            var mergedWaypoints = new WaypointMerger().Merge(simbrief);

            var flight = new FlightMerger().Merge(toolkit, simbrief, blackbox);
            flight.Airports = mergedAirports;
            flight.Landing = mergedLanding;
            flight.Blackbox = mergedBlackBox;
            flight.Waypoints = mergedWaypoints;

            new JsonWriter<Domain.Merged.Flight>().Write(flight, "Flight", flight.Departure, flight.Arrival);
        }
    }
}
