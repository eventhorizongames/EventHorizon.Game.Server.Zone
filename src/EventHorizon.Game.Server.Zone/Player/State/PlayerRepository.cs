using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;

namespace EventHorizon.Game.Server.Zone.Player.State
{
    public class PlayerRepository : IPlayerRepository
    {
        readonly IEntityRepository _entityRepository;

        public PlayerRepository(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<PlayerEntity> FindById(string id)
        {
            var entityList = await _entityRepository.All();
            return entityList
                .FindAll(a => a.Type == EntityType.PLAYER)
                .Cast<PlayerEntity>()
                .FirstOrDefault(a => a.PlayerId == id);
        }

        public Task Remove(PlayerEntity player)
        {
            return _entityRepository.Remove(player.Id);
        }

        public Task Update(EntityAction action, PlayerEntity player)
        {
            return _entityRepository.Update(action, player);
        }
    }
}