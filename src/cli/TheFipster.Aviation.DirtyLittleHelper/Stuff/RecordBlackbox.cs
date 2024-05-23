using TheFipster.Aviation.Domain.BlackBox;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Modules.BlackBox;

namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    internal class RecordBlackbox
    {
        private BlackBoxFlight flight;

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
