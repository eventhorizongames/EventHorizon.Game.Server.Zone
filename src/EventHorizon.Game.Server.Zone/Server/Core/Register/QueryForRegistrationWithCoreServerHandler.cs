using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.ServerProperty;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Server.Core.Register
{
    public class QueryForRegistrationWithCoreServerHandler : IRequestHandler<QueryForRegistrationWithCoreServer, bool>
    {
        private readonly IServerProperty _serverProperty;

        public QueryForRegistrationWithCoreServerHandler(
            IServerProperty serverProperty
        )
        {
            _serverProperty = serverProperty;
        }

        public Task<bool> Handle(
            QueryForRegistrationWithCoreServer request,
            CancellationToken cancellationToken
        )
        {
            return Task.FromResult(
                !string.IsNullOrEmpty(
                    _serverProperty.Get<string>(
                        ServerPropertyKeys.SERVER_ID
                    )
                )
            );
        }
    }
}