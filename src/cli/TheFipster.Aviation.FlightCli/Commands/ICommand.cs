﻿using TheFipster.Aviation.FlightCli.Options;

namespace TheFipster.Aviation.FlightCli.Commands
{
    public interface ICommand<T> where T : IOptions
    {
        void Run(T options, IConfig config);
    }
}
