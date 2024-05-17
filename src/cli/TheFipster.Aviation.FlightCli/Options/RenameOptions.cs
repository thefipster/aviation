﻿using CommandLine;
using TheFipster.Aviation.FlightCli.Abstractions;

namespace TheFipster.Aviation.FlightCli.Options
{
    [Verb("rename", HelpText = "Renames imported files into proper format. Only works after post processing is done.")]
    internal class RenameOptions : FlightOptions { }
}