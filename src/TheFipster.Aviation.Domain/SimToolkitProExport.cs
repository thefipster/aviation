using System.Text.Json.Serialization;
using TheFipster.Aviation.Domain.SimToolkitPro;

namespace TheFipster.Aviation.Domain
{
    public class SimToolkitProExport
    {
        [JsonPropertyName("fleet")]
        public List<Fleet> Fleet { get; set; }

        public List<Landing> Landings { get; set; }

        public List<Logbook> Logbook { get; set; }
    }
}
