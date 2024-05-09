using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.Domain.Merged
{
    public class Airport
    {
        public string Icao { get; set; }
        public string Iata { get; set; }
        public string Continent { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Runway { get; set; }
        public int Elevation { get; set; }
        public string Country { get; set; }
        public string Municipality { get; set; }
        public string Region { get; set; }
        public string Type { get; set; }
        public int TransitionAltitude { get; set; }
        public int TransitionLevel { get; set; }
        public string Interaction { get; set; }
    }
}
