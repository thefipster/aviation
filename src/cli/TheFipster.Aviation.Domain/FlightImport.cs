using TheFipster.Aviation.Domain.BlackBox;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Geo;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.Domain.Simbrief.Kml;
using TheFipster.Aviation.Domain.Simbrief.Xml;

namespace TheFipster.Aviation.Domain
{
    public class FlightImport : FlightBase
    {
        /// <summary>
        /// Contains all information generated during a flight.
        ///     - Simbrief dispatch
        ///     - SimToolkitPro SQLite export
        ///     - Blackbox recording
        /// </summary>
        /// <param name="flightNumber">ATC or imaginary flightnumber</param>
        /// <param name="departure">Planned departure airport ICAO</param>
        /// <param name="arrival">Planned arrival airport ICAO</param>
        public FlightImport(string flightNumber, string departure, string arrival)
            : base(FileTypes.FlightJson, departure, arrival)
        {
            FlightNumber = flightNumber;
        }

        public string FlightNumber { get; set; }

        /// <summary>
        /// Contents of the SimToolkitPro SQLite database.
        /// </summary>
        public SimToolkitProFlight? SimToolkitPro { get; set; }
        public bool HasSimToolkitPro => SimToolkitPro != null;

        /// <summary>
        /// Records from the Blackbox recorder.
        /// </summary>
        public ICollection<Record>? Blackbox { get; set; }
        public bool HasBlackbox => Blackbox != null && Blackbox.Any();

        /// <summary>
        /// Contents of the kml route file from Simbrief dispatch.
        /// </summary>
        public SimbriefKmlRaw SimbriefKml { get; set; }
        public bool HasSimbriefKml => SimbriefKml != null;


        /// <summary>
        /// Contents of the xml data file from Simbrief dispatch.
        /// </summary>
        public SimbriefXmlRaw SimbriefXml { get; set; }
        public bool HasSimbriefXml => SimbriefXml != null;

        /// <summary>
        /// Contents of the json data file from Simbrief dispatch.
        /// </summary>
        public SimbriefImport Simbrief { get; set; }
        public bool HasSimbrief => Simbrief != null;

        /// <summary>
        /// Actual flight track.
        /// Generated from blackbox or simtoolkitpro
        /// </summary>
        public IEnumerable<Coordinate> Track { get; set; }
        public bool HasTrack => Track != null;

        /// <summary>
        /// Statistical data from the flight
        /// Generated from the blackbox and simtoolkitpro and simbrief
        /// </summary>
        public Stats Stats { get; set; }
        public bool HasStats => Stats != null;

        /// <summary>
        /// Map connecting screenshots to latlon coordinates.
        /// Generated from the blackbox or simtoolkitpro
        /// </summary>
        public ICollection<GeoTag> Geotags { get; set; }
        public bool HasGeotags => Geotags != null && Geotags.Any();

        /// <summary>
        /// Waypoints of flight events like takeoff, touchdown point, gear up/down, etc.
        /// Generated from the blackbox
        /// </summary>
        public ICollection<Waypoint> Events { get; set; }
        public bool HasEvents => Events != null && Events.Any();
    }
}
