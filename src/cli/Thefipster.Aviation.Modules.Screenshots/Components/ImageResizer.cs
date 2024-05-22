using ImageMagick;

namespace Thefipster.Aviation.Modules.Screenshots.Components
{
    public class ImageResizer
    {
        public void Resize(string filepath, int width, int height, bool overwrite = false)
        {
            var imageOut = filepath.Replace(".png", ".jpg");
            var imagePre = imageOut.Replace("Screenshot", "Preview").Replace("Chart", "ChartPreview");

            using (MagickImage image = new MagickImage(filepath))
            {
                if (!(File.Exists(imageOut) && !overwrite))
                    image.Write(imageOut, MagickFormat.Jpg);

                if (image.Height > image.Width)
                    image.Resize(width, 0);
                else
                    image.Resize(0, height);

                image.Crop(width, height, Gravity.Center);

                if (!(File.Exists(imagePre) && !overwrite))
                    image.Write(imagePre, MagickFormat.Jpg);
            }
        }

        public void Resize(string original, string resized, int width, int height, bool overwrite = false)
        {
            if (!original.Contains(".jpg"))
                throw new ArgumentException("original image needs to be a jpg image.");
            if (!resized.Contains(".jpg"))
                throw new ArgumentException("resized image needs to be a jpg image.");
            if (!File.Exists(original))
                throw new ArgumentException("original image needs to exist.");
            if (File.Exists(resized) && !overwrite)
                return;

            using (MagickImage image = new MagickImage(original))
            {
                if (image.Height > image.Width)
                    image.Resize(width, 0);
                else
                    image.Resize(0, height);

                image.Crop(width, height, Gravity.Center);

                if (!(File.Exists(resized) && !overwrite))
                    image.Write(resized, MagickFormat.Jpg);
            }
        }
    }
}
