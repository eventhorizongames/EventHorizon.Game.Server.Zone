using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.ClientApi;
using EventHorizon.Game.Server.Zone.Core.Exceptions;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using IdentityModel.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Core.Register.Handler
{
    public class UnregisterWithCoreServerHandler : INotificationHandler<UnregisterWithCoreServerEvent>
    {
        private readonly IMediator _mediator;
        private readonly CoreSettings _coreSettings;
        private readonly IServerProperty _serverProperty;

        public UnregisterWithCoreServerHandler(IMediator mediator,
            IOptions<ZoneSettings> zoneSettings,
            IOptions<CoreSettings> coreSettings,
            IServerProperty serverProperty)
        {
            _mediator = mediator;
            _coreSettings = coreSettings.Value;
            _serverProperty = serverProperty;
        }

        public Task Handle(UnregisterWithCoreServerEvent notification, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task Handle(RegisterWithCoreServerEvent notification, CancellationToken cancellationToken)
        {
            await UnregisterWithCoreServer(
                _coreSettings.Server,
                _serverProperty.Get<string>(ServerPropertyKeys.SERVER_ID)
            );
        }
        private async Task UnregisterWithCoreServer(string coreServerAddress, string zoneId)
        {
            await _mediator.Send(new HttpClientPostEvent
            {
                Uri = $"{coreServerAddress}/api/Zone/{zoneId}/Unregister",
                AccessToken = await _mediator.Send(new RequestIdentityAccessTokenEvent()),
            });
        }
    }
}