using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.CoreCli
{
    public class PlainWriter
    {
        public void Write(string flightFolder, string text, FileTypes filetype, string? departure, string? arrival = null, bool overwrite = false)
        {
            string filetypeName, ending;

            switch (filetype)
            {
                case FileTypes.OfpHtml:
                    filetypeName = "OFP";
                    ending = ".html";
                    break;
                default:
                    throw new ApplicationException($"Unknown text file type {filetype}.");
            };

            if (string.IsNullOrWhiteSpace(filetypeName) || string.IsNullOrWhiteSpace(ending))
                throw new ApplicationException($"filetype and ending are not sufficiently filled.");

            var file = string.IsNullOrEmpty(arrival)
                ? $"{departure} - {filetypeName}{ending}"
                : $"{departure} - {arrival} - {filetypeName}{ending}";

            var path = Path.Combine(flightFolder, file);

            if (File.Exists(path) && !overwrite)
                return;

            File.WriteAllText(path, text);
        }
    }
}
