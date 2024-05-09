using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.CoreCli
{
    public class FileSystemFinder : IFileSystemFinder
    {
        public IEnumerable<string> GetFlightFolders(string flightsFolder)
            => Directory.GetDirectories(flightsFolder);

        public string GetFlightFolder(string flightsFolder, string departure, string arrival)
        {
            var search = $"* - {departure} - {arrival}";
            var candidates = Directory.GetDirectories(flightsFolder, search);
            if (!candidates.Any())
                throw new ApplicationException($"Couldn't find flight from {departure} to {arrival}.");

            return candidates.First();
        }

        public Dictionary<string, FileTypes> GetFiles(string flightFolder)
        {
            var flight = new Dictionary<string, FileTypes>();
            var files = Directory.GetFiles(flightFolder);
            foreach (var file in files)
            {
                var filetype = scanFile(file);
                flight.Add(file, filetype);
            }

            return flight;
        }

        public IEnumerable<string> GetFiles(string flightFolder, FileTypes filetype)
        {
            var files = GetFiles(flightFolder);
            foreach (var file in files)
                if (file.Value == filetype)
                    yield return file.Key;
        }

        public int GetLeg(string flightFolder)
        {
            var folder = Path.GetFileName(flightFolder);
            var split = folder.Split('-');
            return int.Parse(split[0].Trim());
        }

        public string GetDeparture(string flightFolder)
        {
            var folder = Path.GetFileName(flightFolder);
            var split = folder.Split('-');
            return split[1].Trim();
        }

        public string GetArrival(string flightFolder)
        {
            var folder = Path.GetFileName(flightFolder);
            var split = folder.Split('-');
            return split[2].Trim();
        }

        private FileTypes scanFile(string file)
        {
            switch (Path.GetExtension(file))
            {
                case ".xml":
                    {
                        return scanXml(file);
                    }
                case ".csv":
                    {
                        return scanCsv(file);
                    }
                case ".pdf":
                    {
                        return scanPdf(file);
                    }
                case ".pln":
                    {
                        return FileTypes.MsfsFlightPlan;
                    }
                case ".html":
                    {
                        return FileTypes.OfpHtml;
                    }
                case ".png":
                    {
                        return FileTypes.Screenshot;
                    }
                case ".kml":
                    {
                        return scanKml(file);
                    }
                case ".json":
                    {
                        return scanJson(file);
                    }
            }

            return FileTypes.Unknown;
        }

        private FileTypes scanJson(string file)
        {
            var generic = new JsonReader<JsonBase>().FromFile(file);
            if (generic != null && generic.FileType != FileTypes.Empty)
                return generic.FileType;

            var blackbox = new JsonReader<BlackBoxFlight>().FromFile(file);
            if (blackbox != null && blackbox.Records != null && blackbox.Records.Count > 0)
                if (blackbox.Records.Count > 1000)
                    return FileTypes.BlackBoxJson;
                else
                    return FileTypes.Error;

            var toolkit = new JsonReader<SimToolkitProFlight>().FromFile(file);
            if (toolkit?.Logbook != null)
                return FileTypes.SimToolkitProJson;

            return FileTypes.Unknown;
        }

        private FileTypes scanKml(string file)
        {
            if (File.ReadLines(file).Any(x => x == "<Point>"))
                return FileTypes.RouteKml;

            return FileTypes.PathKml;
        }

        private FileTypes scanXml(string file)
        {
            var secondLine = File.ReadLines(file).Skip(1).FirstOrDefault();
            if (secondLine == "<OFP>")
                return FileTypes.SimbriefXml;

            return FileTypes.Unknown;
        }

        private FileTypes scanCsv(string file)
        {
            var text = File.ReadLines(file).FirstOrDefault();

            if (text == Const.BlackBoxHeader)
                return FileTypes.BlackBoxCsv;

            return FileTypes.Unknown;
        }

        private FileTypes scanPdf(string file)
        {
            var filename = Path.GetFileName(file);

            if (filename.Contains("OFP") || filename.Contains("_PDF_"))
                return FileTypes.OfpPdf;

            if (filename.Contains("Airport"))
                return FileTypes.ChartAirport;
            if (filename.Contains("Parking"))
                return FileTypes.ChartParking;
            if (filename.Contains("APP"))
                return FileTypes.ChartApproach;
            if (filename.Contains("STAR"))
                return FileTypes.ChartArrival;
            if (filename.Contains("SID"))
                return FileTypes.ChartDeparture;
            if (filename.Contains("Taxi"))
                return FileTypes.ChartTaxi;

            return FileTypes.Chart;
        }
    }
}
