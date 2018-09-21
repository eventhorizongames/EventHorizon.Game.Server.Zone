using System.Collections.Generic;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.ServerAction.Model;

namespace EventHorizon.Game.Server.Zone.ServerAction.State
{
    public interface IServerActionQueue
    {
        Task<IEnumerable<ServerActionEntity>> Take(int take);
        Task Push(ServerActionEntity entity);
    }
}