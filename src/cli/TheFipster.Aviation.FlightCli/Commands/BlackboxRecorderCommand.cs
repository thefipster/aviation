using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.BlackBox;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.FlightCli.Abstractions;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.BlackBox;

namespace TheFipster.Aviation.FlightCli.Commands
{

    internal class BlackboxRecorderCommand : IFlightRequiredCommand<BlackboxRecorderOptions>
    {
        BlackBoxFlight? flight;

        public void Run(BlackboxRecorderOptions options, IConfig config)
        {
            if (config == null)
                throw new MissingConfigException("No config available.");

            Record(options.DepartureAirport, options.ArrivalAirport);

            var folder = new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport);
            new JsonWriter<BlackBoxFlight>().Write(folder, flight, FileTypes.BlackBoxJson, flight.Origin, flight.Destination);
            new BlackBoxCsvWriter().Write(folder, flight, FileTypes.BlackBoxCsv, flight.Origin, flight.Destination);
        }

        public BlackBoxFlight Record(string departure, string arrival)
        {
            flight = new BlackBoxFlight(departure, arrival);

            var recorder = new BlackBoxRecorder();
            recorder.Tick += Recorder_Tick;

            Console.WriteLine("Press Enter to start recording");
            Console.ReadLine();
            recorder.Start();

            Console.ReadLine();
            recorder.Stop();
            recorder.Tick -= Recorder_Tick;

            return flight;
        }

        void Recorder_Tick(object sender, Record e)
        {
            flight.Records.Add(e);

            Console.Clear();
            e.Print();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press Enter to stop recording");
        }

    }
}
