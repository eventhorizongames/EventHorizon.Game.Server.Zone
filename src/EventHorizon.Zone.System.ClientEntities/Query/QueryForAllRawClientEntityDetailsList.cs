namespace EventHorizon.Zone.System.ClientEntities.Query
{
    using EventHorizon.Zone.System.ClientEntities.Model;
    using global::System.Collections.Generic;
    using MediatR;

    public class QueryForAllRawClientEntityDetailsList : IRequest<IEnumerable<ClientEntityDetails>>
    {

    }
}