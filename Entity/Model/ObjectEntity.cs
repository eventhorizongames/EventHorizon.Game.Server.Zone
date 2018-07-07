using EventHorizon.Game.Server.Zone.Core.Model;

namespace EventHorizon.Game.Server.Zone.Entity.Model
{
    public interface IObjectEntity
    {
        long Id { get; set;}
        EntityType Type { get; }
        PositionState Position { get; set; }
        object Data { get; set; }
    }
}