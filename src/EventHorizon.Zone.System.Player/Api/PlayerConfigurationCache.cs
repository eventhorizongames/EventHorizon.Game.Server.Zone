namespace EventHorizon.Zone.System.Player.Api
{
    using EventHorizon.Zone.Core.Model.Entity;


    public interface PlayerConfigurationCache
    {
        ObjectEntityConfiguration PlayerConfiguration { get; }
    }
}
