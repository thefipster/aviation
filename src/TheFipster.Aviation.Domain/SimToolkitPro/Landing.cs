using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.Domain.SimToolkitPro
{
    public class Landing
    {
        public string LocalId { get; set; }

        public string FleetId { get; set; }

        public string FlightId { get; set; }

        public string AircraftName { get; set; }

        public string TouchdownSpeed { get; set; }

        public string TouchdownVerticalSpeed { get; set; }

        public string TouchdownYaw { get; set; }

        public string TouchdownPitch { get; set; }

        public string TouchdownRoll { get; set; }

        public string TouchdownLatitude { get; set; }

        public string TouchdownLongitude { get; set; }

        public string TouchdownGforce { get; set; }

        public string TouchdownHeading { get; set; }

        public string DetectedRunway { get; set; }

        public string DetectedAirfield { get; set; }

        public object Checksum { get; set; }

        public string Deleted { get; set; }

        public string Lastupdated { get; set; }
    }
}
