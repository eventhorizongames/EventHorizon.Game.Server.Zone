using System.Collections.Generic;
using System.Numerics;
using MediatR;

namespace EventHorizon.Game.Server.Zone.Events.Entity.Search
{
    public struct SearchInAreaWithAllTagsEvent : IRequest<IEnumerable<long>>
    {
        public Vector3 SearchPositionCenter { get; set; }
        public int SearchRadius { get; set; }
        public IList<string> TagList { get; set; }
    }
}