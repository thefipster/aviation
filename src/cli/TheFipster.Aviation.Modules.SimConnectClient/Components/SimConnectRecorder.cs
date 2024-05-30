using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TheFipster.Aviation.Modules.SimConnectClient.Components
{
    public class SimConnectRecorder
    {
        public void Do ()
        {
            const int WM_USER_SIMCONNECT = 0x0402;

            object wrapper = new object();
            nint blah = 0;
            var handle = new HandleRef(wrapper, blah);
            var simconnect = new SimConnect("Flight Recorder", handle.Handle, WM_USER_SIMCONNECT, null, 0);
            simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(simconnect_OnRecvOpen);
            simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(simconnect_OnRecvQuit);
            simconnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(simconnect_OnRecvException);
            simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Title", null, SIMCONNECT_DATATYPE.STRING256, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Heading Degrees True", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Ground Altitude", "meters", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            simconnect.RegisterDataDefineStruct<Struct1>(DEFINITIONS.Struct1);
            simconnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(simconnect_OnRecvSimobjectDataBytype);
            Console.Read();
        }

        private void simconnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            Struct1 struct1 = (Struct1)data.dwData[0];
            Console.WriteLine("LAT: " + struct1.latitude.ToString());
            Console.WriteLine("LON: " + struct1.longitude.ToString());
            Console.WriteLine("HDG: " + struct1.trueheading.ToString());
            Console.WriteLine("ALT: " + struct1.groundaltitude.ToString());
        }

        private void simconnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            Console.WriteLine("Exception received");
        }

        private void simconnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Console.WriteLine("Quit received");
        }

        private void simconnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Console.WriteLine("Connection open");
        }
    }

    public enum DEFINITIONS
    {
        Struct1
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Struct1
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
        public string title;
        public double latitude;
        public double longitude;
        public double trueheading;
        public double groundaltitude;
    }
}
