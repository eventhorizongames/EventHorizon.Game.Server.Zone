using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Map.State;

namespace EventHorizon.Game.Server.Zone.State.Impl
{
    public class ServerState : IServerState
    {
        private static MapGraph MAP;

        public Task<MapGraph> Map()
        {
            return Task.FromResult(MAP);
        }
        public Task SetMap(MapGraph map)
        {
            MAP = map;
            return Task.CompletedTask;
        }
    }
}