using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.Domain.SimToolkitPro;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Extensions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.SimToolkitPro.Components;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class CompressTrackCommand : IFlightCommand<CompressTrackOptions>
    {
        public void Run(CompressTrackOptions options, IConfig config)
        {
            Console.WriteLine("Optimizing STKP track file.");

            if (config == null)
                throw new MissingConfigException("No config available.");

            var folders = options.GetFlightFolders(config.FlightsFolder);
            Track track;
            foreach (var folder in folders)
            {
                Console.WriteLine($"\t {folder}");
                try
                {
                    var file = new FlightFileScanner().GetFile(folder, FileTypes.TrackJson);
                    track = new JsonReader<Track>().FromFile(file);
                }
                catch (Exception)
                {
                    Console.WriteLine("\t\t skipping, no track file");
                    continue;
                }

                var compressedTrack = new SimToolkitProCompressor().CompressTrack(track);
                new JsonWriter<Track>().Write(folder, compressedTrack, FileTypes.TrackCompressedJson, compressedTrack.Departure, compressedTrack.Arrival);
            }
        }
    }
}
