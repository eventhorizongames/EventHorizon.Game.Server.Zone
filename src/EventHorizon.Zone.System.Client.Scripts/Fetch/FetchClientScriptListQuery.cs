using System.Collections.Generic;
using EventHorizon.Zone.System.Client.Scripts.Model;
using MediatR;

namespace EventHorizon.Zone.System.Client.Scripts.Fetch
{
    public struct FetchClientScriptListQuery : IRequest<IEnumerable<ClientScript>>
    {

    }
}