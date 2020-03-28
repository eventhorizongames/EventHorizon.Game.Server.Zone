namespace EventHorizon.Zone.System.ClientEntities.Query
{
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using MediatR;

    public struct QueryForAllClientEntityDetailsList : IRequest<IEnumerable<ClientEntityDetails>>
    {

    }
}