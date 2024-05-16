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
        public void Do()
        {
            var imageIn = "E:\\aviation\\Data\\Temp\\PASY - UHPP - Screenshot - 15.png";
            var imageOut = imageIn.Replace(".png", ".jpg");
            var imagePre = imageOut.Replace("Screenshot", "Preview");

            using (MagickImage image = new MagickImage(imageIn))
            {
                image.Write(imageOut, MagickFormat.Jpg);
                image.Resize(0, 300);
                image.Crop(300, 300, Gravity.Center);
                image.Write(imagePre, MagickFormat.Jpg);
            }
        }
    }
}
