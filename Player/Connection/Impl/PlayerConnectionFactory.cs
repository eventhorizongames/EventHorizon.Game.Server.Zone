using System;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Identity;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventHorizon.Game.Server.Core.Player.Connection.Impl
{
    public class PlayerConnectionFactory : IPlayerConnectionFactory
    {
        readonly ILogger _logger;
        readonly ILoggerFactory _loggerFactory;
        readonly IMediator _mediator;
        readonly PlayerSettings _playerSettings;
        readonly IConnectionCache _connectionCache;

        public PlayerConnectionFactory(ILoggerFactory loggerFactory,
            IMediator mediator,
            IOptions<PlayerSettings> playerSettings,
            IConnectionCache connectionCache)
        {
            _logger = loggerFactory.CreateLogger<PlayerConnectionFactory>();

            _loggerFactory = loggerFactory;
            _mediator = mediator;
            _playerSettings = playerSettings.Value;
            _connectionCache = connectionCache;
        }

        public async Task<IPlayerConnection> GetConnection()
        {
            return new PlayerConnection(
                _loggerFactory.CreateLogger<PlayerConnection>(),
                await _connectionCache.GetConnection($"{_playerSettings.Server}/playerBus", options =>
                    {
                        options.AccessTokenProvider = async () => await _mediator.Send(new RequestIdentityAccessTokenEvent());
                    }));
        }
    }
}