using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.DirtyLittleHelper.Stuff
{
    internal class OurAirportsExecuter
    {
        public void Do()
        {
            var importPath = "E:\\aviation\\Data\\OurAirports\\import";
            var processPath = "E:\\aviation\\Data\\OurAirports\\process";
            var exportPath = "E:\\aviation\\Data\\OurAirports\\export";

            new OurAirportsImporter().Do(importPath, processPath);
            new OurAirportsMetaGenerator().Do(processPath, exportPath);
        }
    }
}
