using System.Text.Json.Serialization;
using TheFipster.Aviation.Domain.Geo;
using TheFipster.Aviation.Modules.Jekyll.Components;

namespace TheFipster.Aviation.Modules.Jekyll.Model
{
    internal class UriLocation : Location
    {
        public UriLocation(GeoTag item, int flightNumber) : base(item)
        {
            Uri = MetaInformation.GenerateScreenshotUrl(item.Screenshot, flightNumber);
        }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
