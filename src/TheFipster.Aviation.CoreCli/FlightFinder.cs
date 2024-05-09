﻿using TheFipster.Aviation.CoreCli.Abstractions;
using TheFipster.Aviation.Domain.Exceptions;

namespace TheFipster.Aviation.CoreCli
{
    public class FlightFinder : IFlightFinder
    {
        public IEnumerable<string> GetFlightFolders(string flightsFolder)
            => Directory.GetDirectories(flightsFolder);

        public string GetFlightFolder(string flightsFolder, string departure, string arrival)
        {
            var search = $"* - {departure} - {arrival}";
            var candidates = Directory.GetDirectories(flightsFolder, search);
            if (!candidates.Any())
                throw new ApplicationException($"Couldn't find flight from {departure} to {arrival}.");

            return candidates.First();
        }

        public string GetLatestFile(string folder, string searchPattern = null)
        {
            IEnumerable<string> filepaths = Enumerable.Empty<string>();
            if (string.IsNullOrWhiteSpace(searchPattern))
                filepaths = Directory.GetFiles(folder);
            else
                filepaths = Directory.GetFiles(folder, searchPattern);

            if (!filepaths.Any())
                throw new EmptyResultException("Folder is empty.");

            var files = filepaths.Select(path => new FileInfo(path));
            var orderedFiles = files.OrderByDescending(file => file.LastWriteTime);
            var latestFile = orderedFiles.FirstOrDefault();
            return latestFile.FullName;
        }
    }
}
