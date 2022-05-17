using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("fin", HelpText = "Finishes the paperwork for the flight.")]
    internal class FinishOptions
    {
        [Option(Required = true)]
        public string Flight { get; set; }
    }
}
