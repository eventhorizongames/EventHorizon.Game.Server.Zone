using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Player;
using EventHorizon.Zone.System.Player.Model.Details;
using EventHorizon.Zone.System.Player.Model.Position;

namespace EventHorizon.Zone.System.Player.Mapper
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