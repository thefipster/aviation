using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.BlackBox;
using TheFipster.Aviation.FlightCli.Options;
using TheFipster.Aviation.Modules.BlackBox;

namespace TheFipster.Aviation.FlightCli.Commands
{

    internal class RecorderCommand
    {
        BlackBoxFlight flight;

        internal void Run(RecorderOptions options)
        {
            flight = new BlackBoxFlight(options.DepartureAirport, options.ArrivalAirport);

            var recorder = new Recorder();
            recorder.Tick += Recorder_Tick;

            Console.WriteLine("Press Enter to start recording");
            Console.ReadLine();
            recorder.Start();

            Console.ReadLine();
            recorder.Stop();

            new JsonWriter().Write(flight);
            new CsvWriter().Write(flight);
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
