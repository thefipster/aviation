using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    public class ScreenshotExporter
    {
        public void Export(string flightFolder, string screenshotFolder, bool overwrite = false)
        {
            var flightNumber = new FlightMeta().GetLeg(flightFolder);
            var imageFolder = Path.Combine(screenshotFolder, flightNumber.ToString());
            var tnFolder = Path.Combine(screenshotFolder, flightNumber.ToString(), "tn");
            exportScreenshots(flightFolder, imageFolder, overwrite);
            exportThumbnails(flightFolder, tnFolder, overwrite);
        }

        private void exportScreenshots(string flightFolder, string screenshotFolder, bool overwrite = false)
        {
            var screenshots = new FlightFileScanner().GetFiles(flightFolder, FileTypes.Image);

            if (screenshots.Any() && !Directory.Exists(screenshotFolder))
                Directory.CreateDirectory(screenshotFolder);

            foreach (var file in screenshots)
            {
                var filename = Path.GetFileName(file);
                filename = filename
                    .Replace(" 1.jpg", " 01.jpg")
                    .Replace(" 2.jpg", " 02.jpg")
                    .Replace(" 3.jpg", " 03.jpg")
                    .Replace(" 4.jpg", " 04.jpg")
                    .Replace(" 5.jpg", " 05.jpg")
                    .Replace(" 6.jpg", " 06.jpg")
                    .Replace(" 7.jpg", " 07.jpg")
                    .Replace(" 8.jpg", " 08.jpg")
                    .Replace(" 9.jpg", " 09.jpg")
                    .Replace(" ", string.Empty);
                var newPath = Path.Combine(screenshotFolder, filename);

                if (File.Exists(newPath) && !overwrite)
                    continue;

                File.Copy(file, newPath);
            }
        }

        private void exportThumbnails(string flightFolder, string thumbnailFolder, bool overwrite = false)
        {
            var thumbnails = new FlightFileScanner().GetFiles(flightFolder, FileTypes.Preview);

            if (thumbnails.Any() && !Directory.Exists(thumbnailFolder))
                Directory.CreateDirectory(thumbnailFolder);

            foreach (var file in thumbnails)
            {
                var filename = Path.GetFileName(file);
                filename = filename
                    .Replace(" 1.jpg", " 01.jpg")
                    .Replace(" 2.jpg", " 02.jpg")
                    .Replace(" 3.jpg", " 03.jpg")
                    .Replace(" 4.jpg", " 04.jpg")
                    .Replace(" 5.jpg", " 05.jpg")
                    .Replace(" 6.jpg", " 06.jpg")
                    .Replace(" 7.jpg", " 07.jpg")
                    .Replace(" 8.jpg", " 08.jpg")
                    .Replace(" 9.jpg", " 09.jpg")
                    .Replace(" ", string.Empty);
                var newPath = Path.Combine(thumbnailFolder, filename);

                if (File.Exists(newPath) && !overwrite)
                    continue;

                File.Copy(file, newPath);
            }
        }
    }
}
