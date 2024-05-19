using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;
using TheFipster.Aviation.Domain.Simbrief.Xml;
using YamlDotNet.Core;
using static System.Net.Mime.MediaTypeNames;

namespace TheFipster.Aviation.Modules.Simbrief.Components
{
    public class SimbriefDownloader
    {
        private const string UrlFormatXml = "https://www.simbrief.com/api/xml.fetcher.php?userid={0}";
        private const string UrlFormatJson = "https://www.simbrief.com/api/xml.fetcher.php?userid={0}&json=v2";

        private readonly FlightMeta meta;
        private readonly FlightFileScanner scanner;
        private readonly XmlReader xmlReader;
        private readonly JsonReader<SimbriefXmlRaw> simbriefRawReader;
        private readonly FileDownloader downloader;

        public SimbriefDownloader()
        {
            meta = new FlightMeta();
            scanner = new FlightFileScanner();
            xmlReader = new XmlReader();
            simbriefRawReader = new JsonReader<SimbriefXmlRaw>();
            downloader = new FileDownloader();
        }

        public void DownloadOfp(string flightFolder, string pilotId)
        {
            var departure = meta.GetDeparture(flightFolder);
            var arrival = meta.GetArrival(flightFolder);

            var xmlUrl = string.Format(UrlFormatXml, pilotId);
            var xmlFilename = departure + " - " + arrival + " - " + "Simbrief.xml";
            var xmlFile = Path.Combine(flightFolder, xmlFilename);
            downloader.Download(xmlUrl, xmlFile);

            var jsonUrl = string.Format(UrlFormatJson, pilotId);
            var jsonFilename = departure + " - " + arrival + " - " + "SimbriefImport.json";
            var jsonFile = Path.Combine(flightFolder, jsonFilename);
            downloader.Download(jsonUrl, jsonFile);
        }

        public void DownloadMaps(string flightFolder)
        {
            var departure = meta.GetDeparture(flightFolder);
            var arrival = meta.GetArrival(flightFolder);

            var xmlFile = scanner.GetFile(flightFolder, FileTypes.SimbriefXml);
            var json = xmlReader.ReadToJson(xmlFile);
            var simbrief = simbriefRawReader.FromText(json);

            var baseUrl = simbrief.Ofp.Images.Directory;
            foreach (var image in simbrief.Ofp.Images.Map)
            {
                var imageUrl = baseUrl + image.Link;
                var filename = departure + " - " + arrival + " - " + "Map" + " - " + image.Name + ".gif";
                var filepath = Path.Combine(flightFolder, filename);
                downloader.Download(imageUrl, filepath);
            }
        }

        public void DownloadKml(string flightFolder)
        {
            var departure = meta.GetDeparture(flightFolder);
            var arrival = meta.GetArrival(flightFolder);

            var xmlFile = scanner.GetFile(flightFolder, FileTypes.SimbriefXml);
            var json = xmlReader.ReadToJson(xmlFile);
            var simbrief = simbriefRawReader.FromText(json);

            var baseUrl = simbrief.Ofp.Files.Directory;
            foreach (var file in simbrief.Ofp.Files.File)
            {
                if (file.Name == "Google Earth KML")
                {
                    var fileUrl = baseUrl + file.Link;
                    var filename = departure + " - " + arrival + " - " + "Route.kml";
                    var filepath = Path.Combine(flightFolder, filename);
                    downloader.Download(fileUrl, filepath);
                    break;
                }
            }
        }

        public void DownloadPdf(string flightFolder)
        {
            var departure = meta.GetDeparture(flightFolder);
            var arrival = meta.GetArrival(flightFolder);

            var xmlFile = scanner.GetFile(flightFolder, FileTypes.SimbriefXml);
            var json = xmlReader.ReadToJson(xmlFile);
            var simbrief = simbriefRawReader.FromText(json);

            var baseUrl = simbrief.Ofp.Files.Directory;
            var fileUrl = baseUrl + simbrief.Ofp.Files.Pdf.Link;
            var filename = departure + " - " + arrival + " - " + "OFP.pdf";
            var filepath = Path.Combine(flightFolder, filename);
            downloader.Download(fileUrl, filepath);
        }
    }
}
