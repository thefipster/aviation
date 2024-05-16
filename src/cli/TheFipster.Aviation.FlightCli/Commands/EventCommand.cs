using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Simbrief;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    /// <summary>
    /// Takes the blackbox file and searches for maximum values and flight events outputting the info into the blackbox stats file.
    /// </summary>
    internal class EventCommand
    {
        private HardcodedConfig config;

        public EventCommand(HardcodedConfig config)
        {
            this.config = config;
        }

        internal void Run(EventOptions options)
        {
            Console.WriteLine("Scans the blackbox for configuration events and record values.");
            IEnumerable<string> folders;
            if (string.IsNullOrEmpty(options.DepartureAirport) || string.IsNullOrEmpty(options.ArrivalAirport))
                folders = new FlightFinder().GetFlightFolders(config.FlightsFolder);
            else
                folders = [new FlightFinder().GetFlightFolder(config.FlightsFolder, options.DepartureAirport, options.ArrivalAirport)];


            foreach (var folder in folders)
            {
                Console.Write($"\t {folder}");

                BlackBoxFlight blackbox = null;
                try
                {
                    var blackboxFile = new FlightFileScanner().GetFile(folder, FileTypes.BlackBoxJson);
                    blackbox = new JsonReader<BlackBoxFlight>().FromFile(blackboxFile);
                }
                catch (Exception)
                {
                    Console.WriteLine(" - skipping, no blackbox file.");
                    continue;
                }

                var records = blackbox.Records.ToList();
                var waypoints = new List<Waypoint>();
                var stats = new BlackBoxStats(blackbox.Origin, blackbox.Destination);

                var maxGS = 0;
                var maxAltitude = 0;
                var maxClimb = 0;
                var maxDescent = 0;
                var maxWindspeed = 0;
                var windir = 0;


                for (int i = 1; i < records.Count; i++)
                {
                    var last = records[i - 1];
                    var cur = records[i];
                    bool above = true;
                    bool below = true;

                    if (last.FlapsConfig != cur.FlapsConfig)
                    {
                        var waypoint = new Waypoint($"Flaps {last.FlapsConfig}->{cur.FlapsConfig}", cur.LatitudeDecimals, cur.LongitudeDecimals);
                        waypoints.Add(waypoint);
                    }

                    if (last.GearPosition != cur.GearPosition)
                    {
                        var waypoint = new Waypoint($"Gear {cur.GearPosition}", cur.LatitudeDecimals, cur.LongitudeDecimals);
                        waypoints.Add(waypoint);
                    }

                    if (last.OnGroundFlag != cur.OnGroundFlag)
                    {
                        var waypoint = new Waypoint(cur.OnGroundFlag ? "Landing" : "Takeoff", cur.LatitudeDecimals, cur.LongitudeDecimals);
                        waypoints.Add(waypoint);
                    }

                    if (above && last.AltimeterFeet < 10000 && cur.AltimeterFeet >= 10000)
                    {
                        var waypoint = new Waypoint("Above 10.000 feet", cur.LatitudeDecimals, cur.LongitudeDecimals);
                        waypoints.Add(waypoint);
                        above = false;
                    }

                    if (below && last.AltimeterFeet >= 10000 && cur.AltimeterFeet < 10000)
                    {
                        var waypoint = new Waypoint("Below 10.000 feet", cur.LatitudeDecimals, cur.LongitudeDecimals);
                        waypoints.Add(waypoint);
                        below = false;
                    }

                    if (cur.GpsAltitudeMeters > maxAltitude)
                        maxAltitude = cur.GpsAltitudeMeters;

                    if (cur.GroundSpeedMps > maxGS)
                        maxGS = cur.GroundSpeedMps;

                    if (cur.VerticalSpeedMps > maxClimb)
                        maxClimb = cur.VerticalSpeedMps;

                    if (cur.VerticalSpeedMps < maxDescent)
                        maxDescent = cur.VerticalSpeedMps;

                    if (cur.WindSpeedKnots > maxWindspeed)
                    {
                        maxWindspeed = cur.WindSpeedKnots;
                        windir = cur.WindDirectionRadians;
                    }
                }

                stats.MaxAltitudeM = maxAltitude;
                stats.MaxGroundSpeedMps = maxGS;
                stats.MaxClimbMps = maxClimb;
                stats.MaxDescentMps = maxDescent;
                stats.MaxWindspeed = UnitConverter.KnotsToMetersPerSecond(maxWindspeed);
                stats.WindDirectionRad = windir;

                bool tocTrigger = true;
                bool gsTrigger = true;
                bool climbTrigger = true;
                bool descentTrigger = true;
                bool windTrigger = true;

                for (int i = 0; i  < records.Count; i++)
                {
                    var rec = records[i];
                    if (tocTrigger && maxAltitude * 0.999 < rec.GpsAltitudeMeters)
                    {
                        var waypoint = new Waypoint($"Real TOC", rec.LatitudeDecimals, rec.LongitudeDecimals);
                        waypoints.Add(waypoint);
                        tocTrigger = false;
                    }

                    if (gsTrigger && rec.GroundSpeedMps == maxGS)
                    {
                        var waypoint = new Waypoint($"Max GS", rec.LatitudeDecimals, rec.LongitudeDecimals);
                        waypoints.Add(waypoint);
                        gsTrigger = false;
                    }

                    if (climbTrigger && rec.VerticalSpeedMps == maxClimb)
                    {
                        var waypoint = new Waypoint($"Max Climb", rec.LatitudeDecimals, rec.LongitudeDecimals);
                        waypoints.Add(waypoint);
                        climbTrigger = false;
                    }

                    if (descentTrigger && rec.VerticalSpeedMps == maxDescent)
                    {
                        var waypoint = new Waypoint($"Max Descent", rec.LatitudeDecimals, rec.LongitudeDecimals);
                        waypoints.Add(waypoint);
                        descentTrigger = false;
                    }

                    if (windTrigger && rec.WindSpeedKnots == maxWindspeed)
                    {
                        var waypoint = new Waypoint($"Max Wind {rec.WindDirectionRadians}/{rec.WindSpeedKnots}", rec.LatitudeDecimals, rec.LongitudeDecimals);
                        waypoints.Add(waypoint);
                        windTrigger = false;
                    }

                }

                stats.Waypoints = waypoints;
                new JsonWriter<BlackBoxStats>().Write(folder, stats, FileTypes.BlackBoxStatsJson, stats.Departure, stats.Arrival);

                Console.WriteLine();
            }
        }
    }
}
