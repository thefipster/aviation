using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    internal class FinishCommand
    {
        internal void Run(FinishOptions o)
        {
            Console.WriteLine(o.Flight);
        }
    }
}
