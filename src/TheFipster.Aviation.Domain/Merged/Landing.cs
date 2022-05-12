using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.Domain.Merged
{
    public class Landing
    {
        public string Speed { get; set; }
        public string VerticalSpeed { get; set; }
        public string Yaw { get; set; }
        public string Pitch { get; set; }
        public string Roll { get; set; }
        public string Heading { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Icao { get; set; }
    }
}
