using System.Globalization;
using System.Text;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.BlackBox;

namespace TheFipster.Aviation.Modules.BlackBox
{
    public class CsvWriter
    {
        private string header = "Timestamp;Altimeter;Bank Angle;Brakes;Heading;Elevation;Engine1N1;Engine1N2;Engine2N1;Engine2N2;Flaps;Fuel;Gear;GpsAltitude;GS;IAS;Lat;Lon;OnGround;OAT;Pitch;RadioAltimeter;TAT;TAS;VS;WindDirection;WindSpeed";

        public void Write(BlackBoxFlight flight)
        {
            var location = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var dir = "Aviation";

            var path = Path.Combine(location, dir);
            Write(path, flight);
        }
        public void Write(string path, BlackBoxFlight flight)
        {
            var file = $"{flight.Origin} - {flight.Destination} - BlackBox.csv";
            var filepath = Path.Combine(path, file);

            var sb = new StringBuilder();
            sb.AppendLine(header);

            foreach (var record in flight.Records)
            {
                var line = makeLine(record);
                sb.AppendLine(line);
            }

            var text = sb.ToString();
            File.WriteAllText(filepath, text);
        }

        private string makeLine(Record record)
        {
            var sb = new StringBuilder();

            sb.Append(record.Timestamp);
            sb.Append(";");
            sb.Append(record.AltimeterFeet);
            sb.Append(";");
            sb.Append(record.BankAngle.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.BrakesActivated);
            sb.Append(";");
            sb.Append(record.CompassHeadingRadians);
            sb.Append(";");
            sb.Append(record.ElevationMeters);
            sb.Append(";");
            sb.Append(record.Engine1N1Percent.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.Engine1N2Percent.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.Engine2N1Percent.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.Engine2N2Percent.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.FlapsConfig);
            sb.Append(";");
            sb.Append(record.FuelLiters);
            sb.Append(";");
            sb.Append(record.GearPosition);
            sb.Append(";");
            sb.Append(record.GpsAltitudeMeters);
            sb.Append(";");
            sb.Append(record.GroundSpeedMps);
            sb.Append(";");
            sb.Append(record.IndicatedAirSpeedKnots);
            sb.Append(";");
            sb.Append(record.LatitudeDecimals.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.LongitudeDecimals.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.OnGroundFlag);
            sb.Append(";");
            sb.Append(record.OutsideAirTemperatureCelsius);
            sb.Append(";");
            sb.Append(record.PitchAngle.ToString(CultureInfo.InvariantCulture));
            sb.Append(";");
            sb.Append(record.RadioAltimeterMeters);
            sb.Append(";");
            sb.Append(record.TotalAirTemperatureCelsius);
            sb.Append(";");
            sb.Append(record.TrueAirSpeedKnots);
            sb.Append(";");
            sb.Append(record.VerticalSpeedMps);
            sb.Append(";");
            sb.Append(record.WindDirectionRadians);
            sb.Append(";");
            sb.Append(record.WindSpeedKnots);

            return sb.ToString();
        }
    }
}
