using System.Collections.Generic;

namespace EventHorizon.Zone.Core.Model.DirectoryService
{
    public interface DirectoryResolver
    {
        IEnumerable<string> GetDirectories(string path);
        IEnumerable<string> GetFiles(string path);
    }
}