namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    internal class FileMover
    {
        public void Do()
        {
            var files = Directory.GetFiles("C:\\Users\\felix\\Source\\aviation\\src\\frontend\\flight-blog\\assets\\images\\screenshots");
            var flights = Directory.GetDirectories("E:\\aviation\\Worldtour\\Flights");

            foreach (var file in files)
            {
                var search = Path.GetFileName(file).Substring(0, 11);
                var flight = flights.First(x => Path.GetFileName(x).Contains(search));
                var newFile = Path.Combine(flight, Path.GetFileName(file));

                Console.WriteLine(newFile);

                if (!File.Exists(newFile))
                    File.Move(file, newFile);
            }
        }
    }
}
