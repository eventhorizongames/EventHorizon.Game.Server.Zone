namespace EventHorizon.Zone.System.ClientEntities.Query
{
    using EventHorizon.Zone.Core.Model.Entity;
    using global::System.Collections.Generic;
    using MediatR;

    public struct QueryForAllClientEntityDetailsList : IRequest<IEnumerable<IObjectEntity>>
    {

    }
}