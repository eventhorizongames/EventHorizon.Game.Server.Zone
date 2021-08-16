namespace EventHorizon.Zone.System.Player.Mapper
{
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Player.Model.Details;

    using global::System.Collections.Generic;

    public static class PlayerFromDetailsToEntity
    {
        public static PlayerEntity MapToNew(
            PlayerDetails details
        )
        {
            var entity = new PlayerEntity
            {
                Id = -1,
                PlayerId = details.Id,
                Name = details.Name,
                Locale = details.Locale,
                ConnectionId = GetConnectionId(details),
                Type = EntityType.PLAYER,
                Transform = details.Transform,
                RawData = details.Data,
                TagList = new List<string> { "player" }
            };
            entity.SetProperty(
                LocationState.PROPERTY_NAME,
                LocationState.New(
                    details.Location.CurrentZone,
                    details.Location.ZoneTag
                )
            );
            return entity;
        }

        private static string GetConnectionId(PlayerDetails details)
        {
            if (details.Data.ContainsKey("ConnectionId"))
            {
                return (string)details.Data["ConnectionId"];
            }
            return "";
        }
    }
}
