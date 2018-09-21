using EventHorizon.Game.Server.Zone.Load.Map.Model;

namespace EventHorizon.Game.Server.Zone.Load.Map
{
    public interface IZoneMapFactory
    {
        ZoneMap Map { get; }
    }
}