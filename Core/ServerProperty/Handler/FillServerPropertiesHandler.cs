using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Core.ClientApi;
using EventHorizon.Game.Server.Zone.Core.Exceptions;
using EventHorizon.Game.Server.Zone.Core.Register.Model;
using EventHorizon.Game.Server.Zone.Core.ServerProperty;
using IdentityModel.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EventHorizon.Game.Server.Zone.Core.ServerProperty.Handler
{
    public class FillServerPropertiesHandler : INotificationHandler<FillServerPropertiesEvent>
    {
        private readonly IServerProperty _serverProperty;
        private readonly IConfiguration _configuration;

        public FillServerPropertiesHandler(IServerProperty serverProperty, IConfiguration configuration)
        {
            _serverProperty = serverProperty;
            _configuration = configuration;
        }

        public Task Handle(FillServerPropertiesEvent notification, CancellationToken cancellationToken)
        {
            _serverProperty.Set(ServerPropertyKeys.HOST, _configuration["VIRTUAL_HOST"]);
            return Task.CompletedTask;
        }
    }
}