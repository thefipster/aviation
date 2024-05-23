using TheFipster.Aviation.Domain.BlackBox;
using TheFipster.Aviation.Modules.BlackBox.Models;

namespace TheFipster.Aviation.Modules.BlackBox
{
    public class BlackBoxRecorder
    {
        private Timer timer;
        private readonly Telemetry telemetry;

        public delegate void TickHandler(object sender, Record e);
        public event TickHandler? Tick;

        private bool isCancelled;

        public BlackBoxRecorder()
        {
            telemetry = new Telemetry();
            timer = new Timer(tick, null, Timeout.Infinite, Timeout.Infinite);
        }

        public void Start()
        {
            timer.Change(TimeSpan.FromSeconds(1), TimeSpan.Zero);
        }

        public void Stop() => isCancelled = true;

        private void tick(object? state)
        {
            var record = telemetry.Get();
            Tick?.Invoke(this, record);

            if (isCancelled)
                timer.Change(Timeout.Infinite, Timeout.Infinite);
            else
                timer.Change(TimeSpan.FromSeconds(1), TimeSpan.Zero);
        }
    }
}
