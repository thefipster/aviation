using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.Domain
{
    public class JsonBase
    {
        public JsonBase() { }

        public JsonBase(FileTypes filetype)
            => FileType = filetype;

        public FileTypes FileType { get; set; }
    }
}
