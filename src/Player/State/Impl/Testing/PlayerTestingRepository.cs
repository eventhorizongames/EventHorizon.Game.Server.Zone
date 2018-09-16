using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Entity.State;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;
using EventHorizon.Game.Server.Zone.Player.Model;

namespace EventHorizon.Game.Server.Zone.Player.State.Impl.Testing
{
    public class PlayerTestingRepository : IPlayerRepository
    {
        readonly IEntityRepository _entityRepository;

        public PlayerTestingRepository(IEntityRepository entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public async Task<PlayerEntity> FindById(string id)
        {
            var entityList = await _entityRepository.All();
            var entity = entityList
                .FindAll(a => a.Type == EntityType.PLAYER)
                .Cast<PlayerEntity>()
                .FirstOrDefault(a => a.PlayerId == id);
            if (!entity.IsFound())
            {
                entity = (PlayerEntity)(await _entityRepository.Add(this.CreateNewTestEntity(id)));
            }
            return entity;
        }

        public Task Remove(PlayerEntity player)
        {
            return _entityRepository.Remove(player.Id);
        }

        public Task Update(EntityAction action, PlayerEntity player)
        {
            return _entityRepository.Update(action, player);
        }

        private PlayerEntity CreateNewTestEntity(string id)
        {
            return new PlayerEntity
            {
                Id = -1,
                PlayerId = id,
                ConnectionId = "",
                Type = EntityType.PLAYER,
                Position = new PositionState
                {
                    CurrentPosition = Vector3.Zero,
                    NextMoveRequest = DateTime.Now.AddMilliseconds(MoveConstants.MOVE_DELAY_IN_MILLISECOND),
                    MoveToPosition = Vector3.Zero,
                    CurrentZone = "",
                    ZoneTag = "testing",
                },
                TagList = new List<string> { "player" },
                Data = new { },
            };
        }
    }
}