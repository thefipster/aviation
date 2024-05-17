﻿using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("trim", HelpText = "Trims the black box files to contain only engine on section.")]
    internal class TrimOptions : FlightOptions { }
}
