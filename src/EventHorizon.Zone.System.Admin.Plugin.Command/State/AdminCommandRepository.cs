using System.Collections.Generic;

using EventHorizon.Zone.System.Admin.Plugin.Command.Model;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.State
{
    public interface AdminCommandRepository
    {
        void Clear();
        void Add(
            AdminCommandInstance adminCommandInstance
        );
        IEnumerable<AdminCommandInstance> Where(
            string command
        );
    }
}
