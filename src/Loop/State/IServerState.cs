using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Map;

namespace EventHorizon.Game.Server.Zone.Loop.State
{
    public interface IServerState
    {
        Task<MapGraph> Map();
         Task SetMap(MapGraph map);
    }
}