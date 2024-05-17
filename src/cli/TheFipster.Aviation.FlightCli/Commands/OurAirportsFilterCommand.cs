using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.OurAirports;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.OurAirports.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Takes the OurAirports csv export files combines them into a single dataset, filters the airport and outputs the remainder as a single json file.
    /// </summary>
    internal class OurAirportsFilterCommand : ICommand<OurAirportsFilterOptions>
    {
        private const string AirportFile = "ourairports.json";
        private const string MetaFile = "airport-meta.json";

        public void Run(OurAirportsFilterOptions options, IConfig config)
        {
            Console.WriteLine("Executing our airports filter command:");
            Console.WriteLine();

            Console.WriteLine("\t importing csv files");
            var airports = new OurAirportsImporter().Filter(
                options.ImportFolder, 
                1500, 
                ["WATER", "TURF", "GRASS", "DIRT", "WAT"],
                true);

            var airportsFile = Path.Combine(options.OutputFolder, AirportFile);
            Console.WriteLine("\t writing filtered output into " + airportsFile);
            new JsonWriter<IEnumerable<OurAirport>>().Write(airportsFile, airports);

            Console.WriteLine("\t generating meta data");
            var meta = airports.Select(x => new AirportMeta(x));
            var metaFile = Path.Combine(options.OutputFolder, MetaFile);
            Console.WriteLine("\t writing meta data into " + metaFile);
            new JsonWriter<IEnumerable<OurAirport>>().Write(metaFile, meta);
        }
    }
}
