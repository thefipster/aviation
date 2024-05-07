using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.BlackBox;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.BlackBox;

namespace TheFipster.Aviation.FlightCli.Commands
{

    internal class RecorderCommand
    {
        BlackBoxFlight? flight;

        internal void Run(RecorderOptions options)
        {
            Record(options.DepartureAirport, options.ArrivalAirport);
            new JsonWriter<BlackBoxFlight>().Write(flight, "BlackBox", flight.Origin, flight.Destination);
            new CsvWriter().Write(flight);
        }

        internal BlackBoxFlight Record(string departure, string arrival)
        {
            flight = new BlackBoxFlight(departure, arrival);

            var recorder = new Recorder();
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
