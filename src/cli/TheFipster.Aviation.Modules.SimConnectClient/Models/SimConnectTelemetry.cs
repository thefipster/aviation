using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;
using TheFipster.Aviation.Modules.SimConnectClient.Attributes;

namespace TheFipster.Aviation.Modules.SimConnectClient.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal class SimConnectTelemetry
    {
        [SimConnectVariable(Name = "PLANE LATITUDE", Unit = "Degrees", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double Latitude;
        [SimConnectVariable(Name = "PLANE LONGITUDE", Unit = "Degrees", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double Longitude;
        [SimConnectVariable(Name = "PLANE ALTITUDE", Unit = "Feet", Type = SIMCONNECT_DATATYPE.FLOAT64)]
        public double Altitude;
    }
}
