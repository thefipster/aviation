using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.Domain.Enums
{
    public class TimeEvents
    {
        public const string PilotIsReady = "Pilot ready";
        public const string SimbriefDispatch = "Simbrief dispatch";
        public const string Takeoff = "Takeoff";
        public const string Touchdown = "Touchdown";
        public const string TopOfClimb = "Top of climb";
        public const string EngineStart = "Engine start";
        public const string StartRolling = "Rolling";
        public const string ParkPosition = "Parked";
    }
}
