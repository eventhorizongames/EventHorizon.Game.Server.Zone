using EventHorizon.Zone.Core.Model.Id;

namespace EventHorizon.Zone.Core.Id
{
    public class InMemoryStaticIdPool : IdPool
    {
        private static long CURRENT_INDEX = 0;

        public long NextId()
        {
            return CURRENT_INDEX++;
        }
    }
}