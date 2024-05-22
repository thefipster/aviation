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

        public byte[] Download(string url)
        {
            using (var client = new HttpClient())
            using (var s = client.GetStreamAsync(url))
            {
                var bin = new byte[s.Result.Length];
                s.Result.Read(bin, 0, bin.Length);
                return bin;
            }
        }

        public string DownloadAsText(string url)
        {
            using (var client = new HttpClient())
            using (var s = client.GetStreamAsync(url))
            {
                var result = s.Result;
                using (var r = new StreamReader(result))
                {
                    var text = r.ReadToEnd();
                    return text;
                }
            }
        }
    }
}
