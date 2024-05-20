using FSUIPC;
using TheFipster.Aviation.Domain.BlackBox;

namespace TheFipster.Aviation.Modules.BlackBox.Models
{
    public class Telemetry
    {
        // Speeds
        private Offset<uint> indicatedAirSpeed = new Offset<uint>(0x02BC);
        private Offset<uint> groundSpeed = new Offset<uint>(0x02B4);
        private Offset<uint> trueAirSpeed = new Offset<uint>(0x02B8);
        private Offset<int> verticalSpeed = new Offset<int>(0x02C8);

        // Altitudes and Elevation
        private Offset<int> altimeter = new Offset<int>(0x3324);
        private Offset<int> radioAltitude = new Offset<int>(0x31E4);
        private Offset<double> gpsAltitude = new Offset<double>(0x6020);
        private Offset<short> groundAltitude = new Offset<short>(0x0B4C);

        // States
        private Offset<int> flaps = new Offset<int>(0x0BDC);
        private Offset<int> gear = new Offset<int>(0x0BE8);
        private Offset<short> parkingBrake = new Offset<short>(0x0BCA);
        private Offset<ushort> onGround = new Offset<ushort>(0x0366);

        // Angles
        private Offset<double> bank = new Offset<double>(0x2F78);
        private Offset<double> pitch = new Offset<double>(0x2F70);
        private Offset<double> compass = new Offset<double>(0x02CC);

        // Position
        private Offset<double> latitude = new Offset<double>(0x6010);
        private Offset<double> longitude = new Offset<double>(0x6018);

        // Weather
        private Offset<ushort> windspeed = new Offset<ushort>(0x0E90);
        private Offset<ushort> winddirection = new Offset<ushort>(0x0E92);
        private Offset<short> oat = new Offset<short>(0x0E8C);
        private Offset<short> tat = new Offset<short>(0x11D0);

        // Fuel and Engines
        private Offset<int> fuel = new Offset<int>(0x1264);
        private Offset<ushort> engine1N1 = new Offset<ushort>(0x0898);
        private Offset<ushort> engine1N2 = new Offset<ushort>(0x0896);
        private Offset<ushort> engine2N1 = new Offset<ushort>(0x0930);
        private Offset<ushort> engine2N2 = new Offset<ushort>(0x092E);


        public bool IsConnected => FSUIPCConnection.IsOpen;

        public Record Get()
        {
            if (!IsConnected)
                connect();

            refresh();
            var record = sanitize();
            applyTimestamp(record);
            return record;
        }
        private void connect() => FSUIPCConnection.Open();

        private static void refresh() => FSUIPCConnection.Process();

        private void applyTimestamp(Record record)
        {
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            record.Timestamp = timestamp;
        }

        private Record sanitize()
        {
            var record = new Record();

            record.IndicatedAirSpeedKnots = (int)Math.Round(indicatedAirSpeed.Value / 128d);
            record.TrueAirSpeedKnots = (int)Math.Round(trueAirSpeed.Value / 128d);
            record.GroundSpeedMps = (int)Math.Round(groundSpeed.Value / 65536d);
            record.VerticalSpeedMps = (int)Math.Round(verticalSpeed.Value / 256d);

            record.AltimeterFeet = altimeter.Value;
            record.GpsAltitudeMeters = (int)Math.Round(gpsAltitude.Value);
            record.RadioAltimeterMeters = radioAltitude.Value / 65536;
            record.ElevationMeters = groundAltitude.Value;

            record.LatitudeDecimals = Math.Round(latitude.Value, 6);
            record.LongitudeDecimals = Math.Round(longitude.Value, 6);

            record.WindSpeedKnots = windspeed.Value;
            record.WindDirectionRadians = (int)Math.Round(winddirection.Value * 360d / 65536d);

            record.OutsideAirTemperatureCelsius = oat.Value / 256;
            record.TotalAirTemperatureCelsius = tat.Value / 256;

            record.BankAngle = Math.Round(bank.Value, 1);
            record.PitchAngle = Math.Round(pitch.Value, 1);
            record.CompassHeadingRadians = (int)Math.Round(compass.Value);

            record.OnGroundFlag = onGround.Value == 0 ? false : true;
            record.FlapsConfig = getFlapsConfig();
            record.GearPosition = gear.Value < 10000 ? "up" : "down";
            record.BrakesActivated = getBrakesActivated();

            record.FuelLiters = (int)Math.Round(fuel.Value * 3.78541);
            record.Engine1N1Percent = Math.Round(engine1N1.Value / 16384d * 100d, 1);
            record.Engine1N2Percent = Math.Round(engine1N2.Value / 16384d * 100d, 1);
            record.Engine2N1Percent = Math.Round(engine2N1.Value / 16384d * 100d, 1);
            record.Engine2N2Percent = Math.Round(engine2N2.Value / 16384d * 100d, 1);

            return record;
        }

        private string getBrakesActivated() => parkingBrake.Value switch
        {
            0 => "none",
            1 => "left",
            2 => "right",
            3 => "both",
            _ => "unknown"
        };

        private string getFlapsConfig()
        {
            if (flaps.Value > 14000)
                return "4";

            if (flaps.Value > 10000)
                return "3";

            if (flaps.Value > 7000)
                return "2";

            if (flaps.Value > 4000)
                return "1+F";

            if (flaps.Value > 50)
                return "1";

            return "retracted";
        }
    }
}
