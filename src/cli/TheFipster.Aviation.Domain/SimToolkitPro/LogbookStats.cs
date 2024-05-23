using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.Domain.SimToolkitPro
{
    public class LogbookStats
    {
        public int FlightTime { get; set; }
        public int FuelRamp { get; set; }
        public int FuelShutdown { get; set; }
        public int FuelUsed { get; set; }
    }
}
