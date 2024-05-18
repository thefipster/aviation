using Newtonsoft.Json;
using System.Xml;

namespace TheFipster.Aviation.CoreCli
{
    public class XmlReader
    {
        public string ReadToJson(string filepath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);
            var json = JsonConvert.SerializeXmlNode(doc);
            return json;
        }
    }
}
