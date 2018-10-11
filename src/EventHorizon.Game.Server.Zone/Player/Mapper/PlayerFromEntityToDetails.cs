using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Model.Player;

namespace EventHorizon.Game.Server.Zone.Player.Mapper
{
    public class PlayerFromEntityToDetails
    {
        public static PlayerDetails Map(PlayerEntity entity)
        {
            return new PlayerDetails
            {
                Id = entity.PlayerId,
                Position = new PlayerPositionState
                {
                    Position = entity.Position.CurrentPosition,
                    CurrentZone = entity.Position.CurrentZone,
                    ZoneTag = entity.Position.ZoneTag,
                },
                Data = entity.AllData(),
            };
        }
    }
}