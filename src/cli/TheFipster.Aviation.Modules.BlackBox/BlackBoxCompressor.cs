﻿using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.BlackBox;

namespace TheFipster.Aviation.Modules.BlackBox
{
    public class BlackBoxCompressor
    {
        public BlackBoxFlight CompressBlackboxRecords(string folder)
        {
            var blackboxFile = new FlightFileScanner().GetFile(folder, FileTypes.BlackBoxTrimmedJson);
            var blackbox = new JsonReader<BlackBoxFlight>().FromFile(blackboxFile);
            var coordinates = blackbox.Records.ToList();

            removeDuplicates(coordinates);
            compressByAngleAndDistance(coordinates);

            var compressedBlackbox = new BlackBoxFlight(blackbox.Origin, blackbox.Destination, coordinates);
            return compressedBlackbox;
        }

        private static void compressByAngleAndDistance(List<Record> coordinates)
        {
            for (int i = coordinates.Count() - 2; i > 0; i--)
            {

                var bearing1 = GpsCalculator.GetBearing(coordinates[i - 1].LatitudeDecimals, coordinates[i - 1].LongitudeDecimals, coordinates[i].LatitudeDecimals, coordinates[i].LongitudeDecimals);
                var bearing2 = GpsCalculator.GetBearing(coordinates[i].LatitudeDecimals, coordinates[i].LongitudeDecimals, coordinates[i + 1].LatitudeDecimals, coordinates[i + 1].LongitudeDecimals);
                var angle = Math.Abs(bearing1 - bearing2);

                var distance = GpsCalculator.GetHaversineDistance(coordinates[i].LatitudeDecimals, coordinates[i].LongitudeDecimals, coordinates[i + 1].LatitudeDecimals, coordinates[i + 1].LongitudeDecimals);
                if (angle < 10 && distance < 10)
                    coordinates.RemoveAt(i);
            }
        }

        private static void removeDuplicates(List<Record> coordinates)
        {
            for (int i = coordinates.Count() - 2; i >= 0; i--)
            {
                if (coordinates[i].Equals(coordinates[i + 1]))
                {
                    coordinates.RemoveAt(i + 1);
                }
            }
        }
    }
}
