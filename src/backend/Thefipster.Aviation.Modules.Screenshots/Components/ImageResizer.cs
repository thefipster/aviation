using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Thefipster.Aviation.Modules.Screenshots.Components
{
    public class ImageResizer
    {
        public void Resize(string filepath, int height, bool overwrite = false)
        {
            if (!File.Exists(filepath))
                throw new FileNotFoundException($"{filepath} doesn't exist.");

            var newFile = filepath.Replace(".png", ".jpg");
            if (File.Exists(newFile) && !overwrite)
                return;

            Image image = Image.FromFile(filepath);
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;

            var scale = (float)height / sourceHeight;
            int destWidth = (int)(sourceWidth * scale);
            int destHeight = (int)(sourceHeight * scale);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, destWidth, destHeight);

            if (File.Exists(newFile))
                File.Delete(newFile);
            b.Save(newFile, ImageFormat.Jpeg);

            g.Dispose();
        }
    }
}
