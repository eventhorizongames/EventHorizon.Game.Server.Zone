using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Connection;
using EventHorizon.Game.Server.Zone.Core.Exceptions;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Core.Register.Model;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Load;
using EventHorizon.Game.Server.Zone.Load.Model;
using EventHorizon.Identity;
using IdentityModel.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Core.Register.Handler
{
    public class RegisterWithCoreServerHandler : INotificationHandler<RegisterWithCoreServerEvent>
    {
        private readonly ILogger _logger;
        private readonly ZoneSettings _zoneSettings;
        private readonly IServerProperty _serverProperty;

        private readonly ICoreConnectionFactory _connectionFactory;

        public RegisterWithCoreServerHandler(ILogger<RegisterWithCoreServerHandler> logger,
            ICoreConnectionFactory connectionFactory,
            IZoneSettingsFactory zoneSettingsFactory,
            IServerProperty serverProperty)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _zoneSettings = zoneSettingsFactory.Settings;
            _serverProperty = serverProperty;
        }

        public async Task Handle(RegisterWithCoreServerEvent notification, CancellationToken cancellationToken)
        {
            var connection = await _connectionFactory.GetConnection();
            var response = await connection.RegisterZone(new ZoneRegistrationDetails
            {
                Tags = _zoneSettings.Tags,
                ServerAddress = _serverProperty.Get<string>(ServerPropertyKeys.HOST)
            });
            _serverProperty.Set(ServerPropertyKeys.SERVER_ID, response.Id);
            _logger.LogInformation("Registered with Core Server: {0}", response.Id);
        }
    }
}