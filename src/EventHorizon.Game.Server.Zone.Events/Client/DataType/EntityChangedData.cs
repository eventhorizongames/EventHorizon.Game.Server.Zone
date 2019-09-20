using System.Numerics;
using EventHorizon.Game.Server.Zone.Client;
using EventHorizon.Zone.Core.Model.Client;
using EventHorizon.Zone.Core.Model.Entity;

namespace EventHorizon.Game.Server.Zone.Client.DataType
{
    public struct EntityChangedData : IClientActionData
    {
        public IObjectEntity Details { get; }
        public EntityChangedData(
            IObjectEntity entity
        ) {
            Details = entity;
        }
    }
}