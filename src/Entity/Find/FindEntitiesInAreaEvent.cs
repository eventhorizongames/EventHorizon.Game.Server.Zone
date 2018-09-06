using System.Collections.Generic;
using System.Numerics;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Entity.Find
{
    public class FindEntitiesInAreaEvent : IRequest<IEnumerable<long>>
    {
        public Vector3 SearchPositionCenter { get; set; }
        public int SearchRadius { get; set; }
    }
}