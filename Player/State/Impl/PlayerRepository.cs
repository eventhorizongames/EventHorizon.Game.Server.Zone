using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Player.Model;

namespace EventHorizon.Game.Server.Zone.Player.State.Impl
{
    public class PlayerRepository : IPlayerRepository
    {
        private static readonly ConcurrentDictionary<string, PlayerEntity> PLAYERS = new ConcurrentDictionary<string, PlayerEntity>();
        public Task<PlayerEntity> FindById(string id)
        {
            return Task.FromResult(
                PLAYERS.FirstOrDefault(a => a.Key == id).Value
            );
        }

        public Task Update(PlayerEntity player)
        {
            PLAYERS.AddOrUpdate(player.Id, player, (key, oldPlayer) => player);
            
            return Task.CompletedTask;
        }
    }
}