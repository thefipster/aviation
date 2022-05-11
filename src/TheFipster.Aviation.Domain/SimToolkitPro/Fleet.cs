using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.Domain.SimToolkitPro
{
    public class Fleet
    {
        public string LocalId { get; set; }

        public string AirframeIcao { get; set; }

        public string AirlineCode { get; set; }

        public string Registration { get; set; }

        public string SimbriefProfileId { get; set; }

        public object SimbriefAcData { get; set; }

        public string LogHours { get; set; }

        public string LogLocation { get; set; }

        public string LogDistance { get; set; }

        public string LogFuelBurned { get; set; }

        public string LogFlightCount { get; set; }

        public string AssociatedLivery { get; set; }

        public string Image { get; set; }

        public object Notes { get; set; }

        public object Checksum { get; set; }

        public string Deleted { get; set; }

        public string Lastupdated { get; set; }
    }
}
