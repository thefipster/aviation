using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    internal class ScreenshotResizer
    {
        public static void Resize(string filepath, int size, bool overwrite = false)
        {
            var imageOut = filepath.Replace(".png", ".jpg");
            var imagePre = imageOut.Replace("Screenshot", "Preview");

            using (MagickImage image = new MagickImage(filepath))
            {
                if (!(File.Exists(imageOut) && !overwrite))
                    image.Write(imageOut, MagickFormat.Jpg);

                if (image.Height > image.Width)
                    image.Resize(size, 0);
                else
                    image.Resize(0, size);

                image.Crop(size, size, Gravity.Center);

                if (!(File.Exists(imagePre) && !overwrite))
                    image.Write(imagePre, MagickFormat.Jpg);
            }
        }
    }
}
