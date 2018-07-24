using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.Connection;
using EventHorizon.Game.Server.Zone.Core.Connection.Stop;
using EventHorizon.Game.Server.Zone.Core.Exceptions;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Core.Register;
using EventHorizon.Game.Server.Zone.Core.Register.Model;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Identity;
using IdentityModel.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Core.Ping.Handler
{
    public class PingCoreServerHandler : INotificationHandler<PingCoreServerEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly ICoreConnectionFactory _connectionFactory;

        public PingCoreServerHandler(ILogger<PingCoreServerHandler> logger,
            IMediator mediator,
            ICoreConnectionFactory connectionFactory)
        {
            _logger = logger;
            _mediator = mediator;
            _connectionFactory = connectionFactory;
        }

        public async Task Handle(PingCoreServerEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var connection = await _connectionFactory.GetConnection();
                await connection.Ping();
                _logger.LogWarning("Zone Core Server Ping.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Zone Core Server Ping failed", ex);
                await _mediator.Publish(new StopZoneCoreConnectionEvent());
                await _mediator.Publish(new RegisterWithCoreServerEvent());
            }
        }
    }
}