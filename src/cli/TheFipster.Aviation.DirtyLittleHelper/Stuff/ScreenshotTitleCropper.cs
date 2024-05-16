using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    internal class ScreenshotTitleCropper
    {
        public void Do()
        {
            var imageIn = "E:\\aviation\\Data\\Temp\\PASY - UHPP - Screenshot - 15.png";
            var imageOut = "E:\\aviation\\Data\\Temp\\PASY - UHPP - Screenshot - 15.png";

            using (MagickImage image = new MagickImage(imageIn))
            {
                var geometry = new MagickGeometry(0, 23, image.Width, image.Height - 23);

                image.Crop(geometry);
                image.Write(imageOut);
            }
        }
    }
}
