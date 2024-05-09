using System.Data.Common;
using System.Data.SQLite;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.SimToolkitPro;

namespace TheFipster.Aviation.Modules.SimToolkitPro.Components
{
    public class SqlReader
    {
        public SimToolkitProFlight Read(string dbFile, string departure, string arrival)
        {
            SQLiteConnection connection;
            connection = new SQLiteConnection($"Data Source={dbFile};Version=3;Compress=True;");

            connection.Open();

            SimToolkitProFlight stkpFlight = readLogbook(departure, arrival, connection);
            var landing = readLanding(connection, stkpFlight.Logbook.LocalId);

            connection.Close();

            stkpFlight.Landing = landing;
            return stkpFlight;
        }

        private Landing readLanding(SQLiteConnection connection, string flightId)
        {
            SQLiteCommand landingCommand = connection.CreateCommand();
            landingCommand.CommandText = $"SELECT * FROM landings WHERE flightId='{flightId}'";

            SQLiteDataReader landingReader;
            landingReader = landingCommand.ExecuteReader();
            Landing landing = new Landing();

            while (landingReader.Read())
            {
                var schema = landingReader.GetColumnSchema();

                foreach (var col in schema)
                {
                    var i = col.ColumnOrdinal;
                    object value;

                    if (!i.HasValue)
                        continue;

                    switch (col.ColumnName)
                    {
                        case "aircraftName":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.AircraftName = value.ToString();
                            break;
                        case "detectedAirfield":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.DetectedAirfield = value.ToString();
                            break;
                        case "detectedRunway":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.DetectedRunway = value.ToString();
                            break;
                        case "fleetId":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.FleetId = value.ToString();
                            break;
                        case "flightId":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.FlightId = value.ToString();
                            break;
                        case "lastupdated":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.Lastupdated = value.ToString();
                            break;
                        case "id":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.LocalId = value.ToString();
                            break;
                        case "touchdownGforce":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.TouchdownGforce = value.ToString();
                            break;
                        case "touchdownHeading":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.TouchdownHeading = value.ToString();
                            break;
                        case "touchdownLatitude":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.TouchdownLatitude = value.ToString();
                            break;
                        case "touchdownLongitude":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.TouchdownLongitude = value.ToString();
                            break;
                        case "touchdownPitch":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.TouchdownPitch = value.ToString();
                            break;
                        case "touchdownRoll":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.TouchdownRoll = value.ToString();
                            break;
                        case "touchdownSpeed":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.TouchdownSpeed = value.ToString();
                            break;
                        case "touchdownVerticalSpeed":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.TouchdownVerticalSpeed = value.ToString();
                            break;
                        case "touchdownYaw":
                            value = landingReader.GetValue(i.Value) ?? string.Empty;
                            landing.TouchdownYaw = value.ToString();
                            break;
                    }
                }
            }

                return landing;
        }

        private static SimToolkitProFlight readLogbook(string departure, string arrival, SQLiteConnection connection)
        {
            SQLiteCommand logbookCommand = connection.CreateCommand();
            logbookCommand.CommandText = $"SELECT * FROM logbook WHERE dep='{departure}' AND arr='{arrival}'";

            SQLiteDataReader logbookReader;
            logbookReader = logbookCommand.ExecuteReader();
            var stkpFlight = new SimToolkitProFlight();
            stkpFlight.Logbook.Dep = departure;
            stkpFlight.Logbook.Arr = arrival;

            while (logbookReader.Read())
            {
                var schema = logbookReader.GetColumnSchema();

                foreach (var col in schema)
                {
                    var i = col.ColumnOrdinal;
                    object value;

                    if (!i.HasValue)
                        continue;

                    switch (col.ColumnName)
                    {
                        case "actualDep":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.ActualDep = value.ToString();
                            break;
                        case "route":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.Route = value.ToString();
                            break;
                        case "actualArr":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.ActualArr = value.ToString();
                            break;
                        case "didDivert":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.DidDivert = value.ToString();
                            break;
                        case "docsRmk":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.DocsRmk = value.ToString();
                            break;
                        case "fleetId":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.FleetId = value.ToString();
                            break;
                        case "fleetReg":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.FleetReg = value.ToString();
                            break;
                        case "flightCallsign":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.FlightCallsign = value.ToString();
                            break;
                        case "flightNumber":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.FlightNumber = value.ToString();
                            break;
                        case "fuelRamp":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.FuelRamp = value.ToString();
                            break;
                        case "fuelShutdown":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.FuelShutdown = value.ToString();
                            break;
                        case "landedAt":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.LandedAt = value.ToString();
                            break;
                        case "lastupdated":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.Lastupdated = value.ToString();
                            break;
                        case "id":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.LocalId = value.ToString();
                            break;
                        case "pausedSeconds":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.PausedSeconds = value.ToString();
                            break;
                        case "trackedGeoJson":
                            value = logbookReader.GetValue(i.Value) ?? string.Empty;
                            stkpFlight.Logbook.TrackedGeoJson = value.ToString();
                            break;
                    }
                }
            }

            return stkpFlight;
        }
    }
}
