using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using EventHorizon.Game.Server.Zone.Admin.Command.Scripts.Model;

namespace EventHorizon.Game.Server.Zone.Admin.Command.Scripts.State
{
    public interface AdminCommandRepository
    {
        void Clear();
        void Add(AdminCommandInstance adminCommandInstance);
        IEnumerable<AdminCommandInstance> Where(string command);
    }

    public class AdminCommandInMemoryRepository : AdminCommandRepository
    {
        private static readonly ConcurrentBag<AdminCommandInstance> INSTANCE_MAP = new ConcurrentBag<AdminCommandInstance>();
        public void Add(AdminCommandInstance command)
        {
            INSTANCE_MAP.Add(command);
        }
        public void Clear()
        {
            INSTANCE_MAP.Clear();
        }
        public IEnumerable<AdminCommandInstance> Where(
            string command
        )
        {
            return INSTANCE_MAP.Where(a => a.Command == command);
        }
    }
}