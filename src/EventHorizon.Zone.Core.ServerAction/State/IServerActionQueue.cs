using System.Collections.Generic;

using EventHorizon.Zone.Core.ServerAction.Model;

namespace EventHorizon.Zone.Core.ServerAction.State
{
    public interface IServerActionQueue
    {
        /// <summary>
        /// Returns and removes based on the take if the Action is a its time to Run.
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<ServerActionEntity> Take(
            int take
        );
        void Push(
            ServerActionEntity entity
        );
    }
}
