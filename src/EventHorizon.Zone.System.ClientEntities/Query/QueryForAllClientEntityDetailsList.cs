namespace EventHorizon.Zone.System.ClientEntities.Query
{
    using global::System.Collections.Generic;
    using MediatR;
    using EventHorizon.Zone.Core.Model.Entity;

    public struct QueryForAllClientEntityDetailsList : IRequest<IEnumerable<IObjectEntity>>
    {

    }
}