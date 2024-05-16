using ImageMagick;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.DirtyLittleHelper.Stuff;
using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.OurAirports;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Started");

        // do your dirty things

        var imageIn = "E:\\aviation\\Data\\Temp\\PASY - UHPP - Screenshot - 15.png";
        Resize(imageIn, 300, true);

        Console.WriteLine("Finished");
    }

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