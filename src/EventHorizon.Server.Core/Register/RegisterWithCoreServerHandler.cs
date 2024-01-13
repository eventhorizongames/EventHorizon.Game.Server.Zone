namespace EventHorizon.Server.Core.Register;

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

public class RegisterWithCoreServerHandler
    : INotificationHandler<RegisterWithCoreServer>
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
        RegisterWithCoreServer notification,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var connection = await _connectionFactory.GetConnection();
            var host = _serverProperty.Get<string>(
                ServerPropertyKeys.HOST
            );
            var applicationVersion = _serverProperty.Get<string>(
                ServerPropertyKeys.APPLICATION_VERSION
            );
            if (string.IsNullOrWhiteSpace(
                host
            ))
            {
                throw new NullReferenceException(
                    "Host was not registered in Server Properties"
                );
            }
            else if (string.IsNullOrWhiteSpace(
                applicationVersion
            ))
            {
                throw new NullReferenceException(
                    "Application Version was not registered in Server Properties"
                );
            }

            var response = await connection.Api
                .RegisterZone(
                    new ZoneRegistrationDetails(
                        host,
                        _zoneSettings.Tag,
                        new ServiceDetails(
                            applicationVersion
                        )
                    )
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
