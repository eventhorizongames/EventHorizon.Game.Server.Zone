namespace EventHorizon.Zone.Core.ServerAction.State
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using EventHorizon.Zone.Core.ServerAction.Model;

    public class ServerActionQueue
        : IServerActionQueue
    {
        private readonly ConcurrentQueue<ServerActionEntity> _actionList = new();
        private readonly ConcurrentBag<ServerActionEntity> _reQueueList = new();

        public void Push(
            ServerActionEntity actionEntity
        )
        {
            _actionList.Enqueue(
                actionEntity
            );
        }

        public IEnumerable<ServerActionEntity> Take(
            int take
        )
        {
            lock (_actionList)
            {
                return Pop(
                    take
                );
            }
        }

        private IEnumerable<ServerActionEntity> Pop(
            int take
        )
        {
            var response = Get(
                new List<ServerActionEntity>(),
                take
            ).AsEnumerable();
            foreach (var action in _reQueueList)
            {
                _actionList.Enqueue(
                    action
                );
            }
            _reQueueList.Clear();
            return response;
        }
        private List<ServerActionEntity> Get(
            List<ServerActionEntity> response,
            int take
        )
        {
            var now = DateTime.UtcNow;
            if (_actionList.TryDequeue(
                out ServerActionEntity serverActionEntity
            ))
            {
                if (
                    now.CompareTo(
                        serverActionEntity.RunAt
                    ) >= 0
                )
                {
                    response.Add(
                        serverActionEntity
                    );
                }
                else
                {
                    _reQueueList.Add(
                        serverActionEntity
                    );
                }
                if (response.Count == take)
                {
                    return response;
                }
                return Get(
                    response,
                    take
                );
            }
            
            return response;
        }
    }
}
