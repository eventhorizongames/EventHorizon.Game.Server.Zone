using EventHorizon.Game.Server.Core.Player.Model;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;

namespace EventHorizon.Game.Server.Zone.Player.Mapper
{
    public class PlayerFromEntityToDetails
    {
        public static PlayerDetails Map(PlayerEntity entity)
        {
            return new PlayerDetails
            {
                Id = entity.PlayerId,
                Name = entity.Name,
                Locale = entity.Locale,
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