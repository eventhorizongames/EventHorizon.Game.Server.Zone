using System.Collections.Generic;
using EventHorizon.Zone.System.ClientEntities.Api;
using MediatR;

namespace EventHorizon.Zone.System.ClientEntities.Fetch
{
    public struct FetchClientEntityInstanceListQuery : IRequest<IEnumerable<IClientEntityInstance>>
    {
        
    }
}