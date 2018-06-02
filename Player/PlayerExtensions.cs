using EventHorizon.Game.Server.Zone.Player.State;
using EventHorizon.Game.Server.Zone.Player.State.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHorizon.Game.Server.Zone.Player
{
    public static class PlayerExtensions
    {
        
        public static void AddPlayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IPlayerRepository, PlayerRepository>();
        }
    }
}