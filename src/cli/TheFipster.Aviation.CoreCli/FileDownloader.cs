namespace TheFipster.Aviation.CoreCli
{
    public class FileDownloader
    {
        public void Download(string url, string filepath)
        {
            using (var client = new HttpClient())
            using (var s = client.GetStreamAsync(url))
            using (var fs = new FileStream(filepath, FileMode.OpenOrCreate))
                s.Result.CopyTo(fs);
        }
    }
}
