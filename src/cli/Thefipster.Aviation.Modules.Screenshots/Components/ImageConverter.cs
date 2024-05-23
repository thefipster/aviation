using ImageMagick;

namespace Thefipster.Aviation.Modules.Screenshots.Components
{
    public class ImageConverter
    {
        public void PngToJpg(string original, string convert, bool overwrite = false)
        {
            if (!original.Contains(".png"))
                throw new ArgumentException("original image needs to be a png image.");
            if (!convert.Contains(".jpg"))
                throw new ArgumentException("converted image needs to be a jpg image.");
            if (!File.Exists(original))
                throw new ArgumentException("original image needs to exist.");
            if (File.Exists(convert) && !overwrite)
                return;

            using (MagickImage image = new MagickImage(original))
                if (!(File.Exists(convert) && !overwrite))
                    image.Write(convert, MagickFormat.Jpg);
        }
    }
}
