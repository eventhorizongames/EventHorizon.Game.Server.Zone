namespace EventHorizon.Zone.System.Admin.Plugin.Command.State
{
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Linq;

    using EventHorizon.Zone.System.Admin.Plugin.Command.Model;

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
            while (INSTANCE_MAP.TryTake(out _)) { }
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
