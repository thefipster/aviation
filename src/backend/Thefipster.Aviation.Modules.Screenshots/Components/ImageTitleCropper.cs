using ImageMagick;
using System.Globalization;

namespace Thefipster.Aviation.Modules.Screenshots.Components
{
    public class ImageTitleCropper
    {
        public void CropTitle(string filepath, bool overwrite = false)
        {
            var file = Path.GetFileName(filepath);
            Console.Write("\t\t" + file);

            var path = Path.GetDirectoryName(filepath);
            if (Directory.GetFiles(path).Any(x => x.Contains(".cropnot")))
            {
                Console.WriteLine(" - skipping, crop lock folder");
                return;

            }

            var safety = filepath.Replace(".png", ".cropped");
            if (File.Exists(safety))
            {
                Console.WriteLine(" - skipping, crop lock file");
                return;
            }

            Console.WriteLine(" - cropping");

            using (MagickImage image = new MagickImage(filepath))
            {
                var geometry = new MagickGeometry(0, 23, image.Width, image.Height - 23);

                image.Crop(geometry);
                image.Write(filepath);
                File.WriteAllText(safety, DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
            }
        }
    }
}
