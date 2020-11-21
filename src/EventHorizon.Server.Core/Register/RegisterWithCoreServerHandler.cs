namespace EventHorizon.Server.Core.Register
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Server.Core.Connection;
    using EventHorizon.Server.Core.Connection.Model;
    using EventHorizon.Server.Core.Events.Register;
    using EventHorizon.Zone.Core.Model.ServerProperty;
    using EventHorizon.Zone.Core.Model.Settings;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class RegisterWithCoreServerHandler : INotificationHandler<RegisterWithCoreServer>
    {
        private readonly ILogger _logger;
        private readonly ZoneSettings _zoneSettings;
        private readonly IServerProperty _serverProperty;
        private readonly CoreServerConnection _connection;

        public RegisterWithCoreServerHandler(
            ILogger<RegisterWithCoreServerHandler> logger,
            ZoneSettings zoneSettings,
            IServerProperty serverProperty,
            CoreServerConnection connection
        )
        {
            _logger = logger;
            _zoneSettings = zoneSettings;
            _serverProperty = serverProperty;
            _connection = connection;
        }

        public async Task Handle(
            RegisterWithCoreServer notification,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var response = await _connection.Api
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
                    ex,
                    "Failed to register with ZoneServer: {Tag} | {Host}",
                    _zoneSettings.Tag,
                    _serverProperty.Get<string>(
                        ServerPropertyKeys.HOST
                    )
                );
                throw;
            }
        }
    }
}