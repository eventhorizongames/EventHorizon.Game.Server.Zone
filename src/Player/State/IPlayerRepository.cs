using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Player.Model;

namespace EventHorizon.Game.Server.Zone.Player.State
{
    public interface IPlayerRepository
    {
        Task<PlayerEntity> FindById(string id);
        Task Update(EntityAction action, PlayerEntity player);
        Task Remove(PlayerEntity player);
    }
}