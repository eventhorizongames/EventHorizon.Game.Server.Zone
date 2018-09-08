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
        private ConcurrentBag<ServerActionEntity> _actionList = new ConcurrentBag<ServerActionEntity>();

        public Task Clear()
        {
            _actionList.Clear();
            return Task.CompletedTask;
        }
        public Task Push(ServerActionEntity actionEntity)
        {
            _actionList.Add(actionEntity);
            return Task.CompletedTask;
        }
        /// <summary>
        /// Returns and removes based on the take if the Action is a its time to Run.
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ServerActionEntity>> Take(int take)
        {
            var returnList = await this.Pop(take);
            await this.Remove(returnList);
            return returnList;
        }
        
        private Task<IEnumerable<ServerActionEntity>> Pop(int take)
        {
            var now = DateTime.UtcNow;
            return Task.FromResult(_actionList
                .Where(a => now.CompareTo(a.RunAt) >= 0)
                .Take(take)
                .AsEnumerable()
            );
        }
        private Task Remove(IEnumerable<ServerActionEntity> deleteList)
        {
            _actionList = new ConcurrentBag<ServerActionEntity>(deleteList);
            return Task.CompletedTask;
        }
    }
}