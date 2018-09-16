using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Map.State;

namespace EventHorizon.Game.Server.Zone.State
{
    public interface IServerState
    {
        Task<MapGraph> Map();
         Task SetMap(MapGraph map);
    }
}