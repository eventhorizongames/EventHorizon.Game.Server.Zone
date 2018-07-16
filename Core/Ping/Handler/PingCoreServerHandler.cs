using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.ClientApi;
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
        private readonly CoreSettings _coreSettings;
        private readonly IServerProperty _serverProperty;

        public PingCoreServerHandler(ILogger<PingCoreServerHandler> logger,
            IMediator mediator,
            IOptions<CoreSettings> coreSettings,
            IServerProperty serverProperty)
        {
            _logger = logger;
            _mediator = mediator;
            _coreSettings = coreSettings.Value;
            _serverProperty = serverProperty;
        }

        public async Task Handle(PingCoreServerEvent notification, CancellationToken cancellationToken)
        {
            await PingCoreServer(
                _coreSettings.Server,
                _serverProperty.Get<string>(ServerPropertyKeys.SERVER_ID)
            );
        }
        private async Task PingCoreServer(string coreServerAddress, string zoneId)
        {
            var response = await _mediator.Send(new HttpClientPostEvent
            {
                Uri = $"{coreServerAddress}/api/Zone/{zoneId}/Ping",
                AccessToken = await _mediator.Send(new RequestIdentityAccessTokenEvent()),
                Content = ""
            });
            if (!response.IsSuccessStatusCode)
            {
                await _mediator.Publish(new RegisterWithCoreServerEvent());
                throw new UnableToPingCoreServerException("Invalid response from Zone Ping request. ZoneId: " + zoneId);
            }
        }
    }
}