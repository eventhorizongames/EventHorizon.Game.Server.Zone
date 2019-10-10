using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EventHorizon.Zone.System.Admin.Plugin.Command.Model.Scripts;

namespace EventHorizon.Zone.System.Admin.Plugin.Command.Scripts.State
{
    public class AdminCommandInMemoryRepository : AdminCommandRepository
    {
        private readonly ConcurrentBag<AdminCommandInstance> INSTANCE_MAP = new ConcurrentBag<AdminCommandInstance>();

        public void Add(
            AdminCommandInstance command
        )
        {
            INSTANCE_MAP.Add(
                command
            );
        }

        public void Clear()
        {
            INSTANCE_MAP.Clear();
        }

        public IEnumerable<AdminCommandInstance> Where(
            string command
        )
        {
            return INSTANCE_MAP.Where(
                adminCommand => adminCommand.Command == command
            );
        }
    }
}