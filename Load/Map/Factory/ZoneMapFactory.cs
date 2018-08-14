
using EventHorizon.Game.Server.Zone.Load.Map.Model;

namespace EventHorizon.Game.Server.Zone.Load.Map.Factory
{
    public class ZoneMapFactory : IZoneMapFactory, IZoneMapSetter
    {
        private static ZoneMap EMPTY_ZONE_MAP = new ZoneMap { };
        public ZoneMap Map { get; private set; }

        public ZoneMapFactory()
        {
            this.Map = EMPTY_ZONE_MAP;
        }

        public void Set(ZoneMap map)
        {
            Map = map;
        }
    }
}