using System;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Server.Core.External.Connection;
using EventHorizon.Server.Core.External.Connection.Model;
using EventHorizon.Zone.Core.Model.Settings;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHorizon.Game.Server.Zone.Core.Register.Handler
{
    public class RegisterWithCoreServerHandler : INotificationHandler<RegisterWithCoreServerEvent>
    {
        private readonly ILogger _logger;
        private readonly ZoneSettings _zoneSettings;
        private readonly IServerProperty _serverProperty;
        private readonly CoreServerConnectionFactory _connectionFactory;

        public RegisterWithCoreServerHandler(
            ILogger<RegisterWithCoreServerHandler> logger,
            ZoneSettings zoneSettings,
            IServerProperty serverProperty,
            CoreServerConnectionFactory connectionFactory
        )
        {
            _logger = logger;
            _zoneSettings = zoneSettings;
            _serverProperty = serverProperty;
            _connectionFactory = connectionFactory;
        }

        public async Task Handle(
            RegisterWithCoreServerEvent notification,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var zoneConnection = await _connectionFactory.GetConnection();
                var response = await zoneConnection
                    .RegisterZone(
                        new ZoneRegistrationDetails
                        {
                            Tag = _zoneSettings.Tag,
                            ServerAddress = _serverProperty.Get<string>(
                                ServerPropertyKeys.HOST
                            )
                        }
                    );
                _serverProperty.Set(
                    ServerPropertyKeys.SERVER_ID,
                    response.Id
                );
                _logger.LogInformation(
                    "Registered with Core Server: {ZoneServerId}",
                    response.Id
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Failed to register with ZoneServer: {Tags} | {Host}",
                    _zoneSettings.Tag,
                    _serverProperty.Get<string>(
                        ServerPropertyKeys.HOST
                    )
                );
                throw ex;
            }
        }
    }
}