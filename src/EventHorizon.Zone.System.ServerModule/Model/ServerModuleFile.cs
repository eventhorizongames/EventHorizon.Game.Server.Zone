using System.Collections.Generic;

namespace EventHorizon.Zone.System.ServerModule.Model
{
    public struct ServerModuleFile
    {
        public string Name { get; set; }
        public IEnumerable<ServerModuleDefintion> ServerModuleList { get; set; }
    }
}