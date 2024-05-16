using TheFipster.Aviation.Domain;
using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.CoreCli.Abstractions
{
    public interface IJsonWriter<T>
    {
        public void Write<T>(
            string flightFolder, 
            T data, 
            FileTypes filetype, 
            string? departure, 
            string? arrival = null, 
            bool overwrite = false
        ) where T : JsonBase;

        public void Write<T>(
            string filepath, 
            FileTypes filetype, 
            T data, 
            bool overwrite = false
        ) where T : JsonBase;

        public void Write<T>(
            string filepath, 
            T data, 
            bool overwrite = false);
    }
}
