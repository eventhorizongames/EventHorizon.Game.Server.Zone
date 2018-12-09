using System.Collections.Generic;
using System.IO;
using EventHorizon.Game.Server.Zone.External.DirectoryService;

namespace EventHorizon.Game.Server.Zone.Core.DirectoryService
{
    public class ServerDirectoryResolver : DirectoryResolver
    {
        public IEnumerable<string> GetFiles(string path)
        {
            return Directory.EnumerateFiles(path);
        }
    }
}