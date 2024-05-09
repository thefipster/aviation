﻿using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.SimToolkitPro;

namespace TheFipster.Aviation.Modules.SimToolkitPro.Components
{
    public class SimToolkitProTrackExtracter
    {
        public Track Extract(string flightPath)
        {
            var files = new FlightFileScanner().GetFiles(flightPath, Domain.Enums.FileTypes.SimToolkitProJson);
            if (!files.Any())
                throw new FileNotFoundException($"No SimToolkitPro JSON in folder {flightPath}.");

            var file = files.First();
            var flight = new JsonReader<SimToolkitProFlight>().FromFile(file);

            Track? track = new JsonReader<Track>().FromText(flight.Logbook.TrackedGeoJson);
            track.Departure = flight.Logbook.Dep;
            track.Arrival = flight.Logbook.Arr;

            return track;
        }
    }
}