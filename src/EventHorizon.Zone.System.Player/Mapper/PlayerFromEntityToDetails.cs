namespace EventHorizon.Zone.System.Player.Mapper
{
    using EventHorizon.Zone.Core.Model.Core;
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.Core.Model.Player;
    using EventHorizon.Zone.System.Player.Model.Details;

    public static class PlayerFromEntityToDetails
    {
        public static PlayerDetails Map(PlayerEntity entity)
        {
            return new PlayerDetails
            {
                Id = entity.PlayerId,
                Name = entity.Name,
                Locale = entity.Locale,
                Transform = entity.Transform,
                Location = entity.GetProperty<LocationState>(
                    LocationState.PROPERTY_NAME
                ),
                Data = entity.AllData(),
            };
        }
    }
}