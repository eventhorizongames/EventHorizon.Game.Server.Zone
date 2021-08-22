namespace EventHorizon.Zone.Core.Events.Entity.Find
{
    using System;
    using System.Collections.Generic;

    using EventHorizon.Zone.Core.Model.Entity;

    using MediatR;

    /// <summary>
    /// Used In Script:
    /// - EntityClearOwner.csx 
    /// </summary>
    public struct QueryForEntities : IRequest<IEnumerable<IObjectEntity>>
    {
        public Func<IObjectEntity, bool> Query { get; set; }
    }
}
