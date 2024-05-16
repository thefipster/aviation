using ImageMagick;

namespace Thefipster.Aviation.Modules.Screenshots.Components
{
    public class PdfConverter
    {
        public void ToImage(string filepath)
        {
            var filename = Path.GetFileName(filepath);
            var indexof = filename.IndexOf(" - ");
            var newFile = filename.Substring(0, indexof) + " - Chart" + filename.Substring(indexof);
            newFile = newFile.Replace(".pdf", ".png");
            var newPath = filepath.Replace(filename, newFile);

            if (File.Exists(newPath) )
                return;

            var settings = new MagickReadSettings
            {
                Density = new Density(300, 300)
            };

            using var images = new MagickImageCollection();

            images.Read(filepath, settings);
            if (images.Count > 1)
            {
                throw new Exception("MORE THAN ONE PAGE");
            }

            foreach (var image in images)
            {
                image.Write(newPath);
            }
        }
    }
}
