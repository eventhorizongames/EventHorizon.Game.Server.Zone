namespace EventHorizon.Zone.System.Admin.Plugin.Command.State
{
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Linq;

    using EventHorizon.Zone.System.Admin.Plugin.Command.Model;

    public class AdminCommandInMemoryRepository : AdminCommandRepository
    {
        private readonly ConcurrentBag<AdminCommandInstance> _map = new();

        public void Add(
            AdminCommandInstance command
        )
        {
            _map.Add(
                command
            );
        }

        public void Clear()
        {
            while (_map.TryTake(out _)) { }
        }

        public IEnumerable<AdminCommandInstance> Where(
            string command
        )
        {
            return _map.Where(
                adminCommand => adminCommand.Command == command
            );
        }
    }
}
