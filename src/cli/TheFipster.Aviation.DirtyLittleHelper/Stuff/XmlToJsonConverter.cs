using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.CoreCli;

namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    internal class XmlToJsonConverter
    {
        public string Convert(string filepath)
            => new XmlReader().ReadToJson(filepath);
    }
}
