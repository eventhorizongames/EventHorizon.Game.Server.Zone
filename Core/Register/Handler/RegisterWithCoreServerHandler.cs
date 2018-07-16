using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.ClientApi;
using EventHorizon.Game.Server.Zone.Core.Exceptions;
using EventHorizon.Game.Server.Zone.Core.Model;
using EventHorizon.Game.Server.Zone.Core.Register.Model;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using EventHorizon.Game.Server.Zone.Load;
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
        private readonly IMediator _mediator;
        private readonly IZoneSettings _zoneSettings;
        private readonly CoreSettings _coreSettings;
        private readonly IServerProperty _serverProperty;

        public RegisterWithCoreServerHandler(ILogger<RegisterWithCoreServerHandler> logger,
            IMediator mediator,
            IZoneSettingsFactory zoneSettingsFactory,
            IOptions<CoreSettings> coreSettings,
            IServerProperty serverProperty)
        {
            _logger = logger;
            _mediator = mediator;
            _zoneSettings = zoneSettingsFactory.Settings;
            _coreSettings = coreSettings.Value;
            _serverProperty = serverProperty;
        }

        public async Task Handle(RegisterWithCoreServerEvent notification, CancellationToken cancellationToken)
        {
            var response = await RegisterWithCoreServer(
                _coreSettings.Server,
                _serverProperty.Get<string>(ServerPropertyKeys.HOST),
                _zoneSettings.Tags
            );
            _serverProperty.Set(ServerPropertyKeys.SERVER_ID, response.Id);
            _logger.LogInformation("Registered with Core Server: {0}", response.Id);
        }
        private async Task<ZoneRegisteredResponse> RegisterWithCoreServer(string coreServerAddress, string host, IList<string> tags)
        {
            var response = await _mediator.Send(new HttpClientPostEvent
            {
                Uri = $"{coreServerAddress}/api/Zone/Register",
                AccessToken = await _mediator.Send(new RequestIdentityAccessTokenEvent()),
                Content = new
                {
                    ServerAddress = host,
                    Tags = tags
                }
            });
            if (!response.IsSuccessStatusCode)
            {
                throw new UnableToRegisterWithCoreServerException("Invalid response from Zone Register request.");
            }
            return JsonConvert.DeserializeObject<ZoneRegisteredResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}