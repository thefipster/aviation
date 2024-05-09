using TheFipster.Aviation.Domain;

namespace TheFipster.Aviation.Modules.Merger
{
    public class LandingMerger
    {
        public Domain.Merged.Landing Merge(SimToolkitProFlight flight)
        {
            var landing = flight.Landing;
            var merge = new Domain.Merged.Landing();

            merge.Speed = landing.TouchdownSpeed;
            merge.VerticalSpeed = landing.TouchdownVerticalSpeed;
            merge.Yaw = landing.TouchdownYaw;
            merge.Pitch = landing.TouchdownPitch;
            merge.Roll = landing.TouchdownRoll;
            merge.Heading = landing.TouchdownHeading;
            merge.Latitude = landing.TouchdownLatitude;
            merge.Longitude = landing.TouchdownLongitude;
            merge.Icao = landing.DetectedAirfield;

            return merge;
        }
    }
}
