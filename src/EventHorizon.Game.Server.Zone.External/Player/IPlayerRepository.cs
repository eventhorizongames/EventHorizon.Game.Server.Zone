using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Model.Player;

namespace EventHorizon.Game.Server.Zone.External.Player
{
    public interface IPlayerRepository
    {
        Task<PlayerEntity> FindById(string id);
        Task Update(EntityAction action, PlayerEntity player);
        Task Remove(PlayerEntity player);
    }
}