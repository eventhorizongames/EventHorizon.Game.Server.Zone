using System.Collections.Generic;

namespace EventHorizon.Game.Server.Zone.External.DirectoryService
{
    public interface DirectoryResolver
    {
        IEnumerable<string> GetDirectories(string path);
        IEnumerable<string> GetFiles(string path);
    }
}