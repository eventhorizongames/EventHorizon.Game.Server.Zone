namespace EventHorizon.Server.Core.Register;

using System.Threading;
using System.Threading.Tasks;

using EventHorizon.Server.Core.Events.Register;
using EventHorizon.Zone.Core.Model.ServerProperty;

using MediatR;

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
        return (!string.IsNullOrEmpty(
            _serverProperty.Get<string>(
                ServerPropertyKeys.SERVER_ID
            )
        )).FromResult();
    }
}
