using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.Domain
{
    public class HardcodedConfig
    {
        public string AirportFile => "E:\\aviation\\Data\\airport-codes.json";
        public string FlightsFolder => "E:\\aviation\\Worldtour\\Flights";
        public string SimbriefFolder => "E:\\aviation\\Data\\Simbrief";
        public string SimToolkitProFolder => "E:\\aviation\\Data\\SimtoolkitPro";
        public string NavigraphFolder => "E:\\aviation\\Data\\Navigraph";
    }
}
