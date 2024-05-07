using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TheFipster.Aviation.Modules.Simbrief.Components
{
    public class Finder
    {
        public string Find(string directory)
        {
            var files = Directory.GetFiles(directory, "*.xml");

            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                var xml = XElement.Load(content);

                if (xml.Name == "OFP")
                    return file;
            }

            throw new ApplicationException($"Couldn't find a simbrief xml file in the directory {directory}");
        }

        public IEnumerable<string> FindExportFiles(string directory, string departureICAO, string arrivalICAO) 
            => Directory.GetFiles(directory, $"{departureICAO}{arrivalICAO}*");
    }
}
