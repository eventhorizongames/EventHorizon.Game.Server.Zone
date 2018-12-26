using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.ServerAction.Model;
using System;

namespace EventHorizon.Game.Server.Zone.ServerAction.State.Impl
{
    public class ServerActionQueue : IServerActionQueue
    {
        private ConcurrentQueue<ServerActionEntity> _actionList = new ConcurrentQueue<ServerActionEntity>();
        private ConcurrentBag<ServerActionEntity> _reQueueList = new ConcurrentBag<ServerActionEntity>();

        public Task Clear()
        {
            _actionList.Clear();
            return Task.CompletedTask;
        }
        public Task Push(ServerActionEntity actionEntity)
        {
            _actionList.Enqueue(actionEntity);
            return Task.CompletedTask;
        }
        /// <summary>
        /// Returns and removes based on the take if the Action is a its time to Run.
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        public Task<IEnumerable<ServerActionEntity>> Take(int take)
        {
            lock (_actionList)
            {
                return Task.FromResult(this.Pop(take));
            }
        }

        private IEnumerable<ServerActionEntity> Pop(int take)
        {
            var response = Get(new List<ServerActionEntity>(), take).AsEnumerable();
            foreach (var action in _reQueueList)
            {
                _actionList.Enqueue(action);
            }
            _reQueueList.Clear();
            return response;
        }
        private List<ServerActionEntity> Get(List<ServerActionEntity> response, int take)
        {
            var now = DateTime.UtcNow;
            var serverActionEntity = default(ServerActionEntity);
            if (_actionList.TryDequeue(out serverActionEntity))
            {
                if (now.CompareTo(serverActionEntity.RunAt) >= 0)
                {
                    response.Add(serverActionEntity);
                }
                else
                {
                    _reQueueList.Add(serverActionEntity);
                }
                if (response.Count == take)
                {
                    return response;
                }
                return Get(response, take);
            }
            else
            {
                return response;
            }
        }
        // private Task Remove(IEnumerable<ServerActionEntity> deleteList)
        // {
        //     _actionList = new ConcurrentBag<ServerActionEntity>(_actionList.Except(deleteList));
        //     return Task.CompletedTask;
        // }
    }
}