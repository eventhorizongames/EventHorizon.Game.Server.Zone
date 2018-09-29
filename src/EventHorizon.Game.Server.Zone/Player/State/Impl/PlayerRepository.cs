using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.External.Entity;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Player.Model;

namespace EventHorizon.Game.Server.Zone.Player.State.Impl
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