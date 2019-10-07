using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace EventHorizon.Game.Server.Zone.Core.ServerProperty.Handler
{
    public class FillServerPropertiesHandler : INotificationHandler<FillServerPropertiesEvent>
    {
        private readonly IServerProperty _serverProperty;
        private readonly IConfiguration _configuration;

        public FillServerPropertiesHandler(
            IServerProperty serverProperty, 
            IConfiguration configuration
        )
        {
            _serverProperty = serverProperty;
            _configuration = configuration;
        }

        public Task Handle(
            FillServerPropertiesEvent notification, 
            CancellationToken cancellationToken
        )
        {
            _serverProperty.Set(
                ServerPropertyKeys.HOST, 
                _configuration["HOST"]
            );
            return Task.CompletedTask;
        }
    }
}