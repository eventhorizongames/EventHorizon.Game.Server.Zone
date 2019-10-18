using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.ServerProperty;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace EventHorizon.Zone.Core.ServerProperty.Fill
{
    public class FillHostServerPropertyHandler : INotificationHandler<FillServerPropertiesEvent>
    {
        private readonly IServerProperty _serverProperty;
        private readonly IConfiguration _configuration;

        public FillHostServerPropertyHandler(
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