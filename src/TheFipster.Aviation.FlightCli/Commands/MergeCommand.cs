using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
            var simbrief = new JsonReader<SimBriefFlight>().Read(options.SimbriefFile);
            var blackbox = new JsonReader<BlackBoxFlight>().Read(options.BlackboxFile);
            var toolkit = new JsonReader<SimToolkitProFlight>().Read(options.ToolkitFile);
            var airports = new List<Airport>();
            foreach (var airportFile in options.AirportFiles)
            {
                var airport = new JsonReader<Airport>().Read(airportFile);
                airports.Add(airport);
            }

            var mergedAirports = new AirportMerger().Merge(airports, simbrief);
            var mergedLanding = new LandingMerger().Merge(toolkit);
            var mergedBlackBox = new BlackBoxMerger().Merge(blackbox);


        }
    }
}
