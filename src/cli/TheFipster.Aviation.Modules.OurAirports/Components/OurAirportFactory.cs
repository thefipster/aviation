using TheFipster.Aviation.Domain.Exceptions;
using TheFipster.Aviation.Domain.OurAirports;

namespace TheFipster.Aviation.Modules.OurAirports.Components
{
    public class OurAirportFactory
    {
        public IOurAirportData Produce<T>(string[] lines) where T: IOurAirportData
        {
            if (typeof(T) == typeof(OurAirport))
                return OurAirport.FromOurAirportsCsv(lines);
            if (typeof(T) == typeof(OurRunway))
                return OurRunway.FromOurAirportsCsv(lines);
            if (typeof(T) == typeof(OurCountry))
                return OurCountry.FromOurAirportsCsv(lines);
            if (typeof(T) == typeof(OurRegion))
                return OurRegion.FromOurAirportsCsv(lines);

            throw new UnknownFileTypeException("Given ourairports type is not known.");
        }
    }
}
