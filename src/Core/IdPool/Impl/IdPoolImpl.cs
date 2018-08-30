namespace EventHorizon.Game.Server.Zone.Core.IdPool.Impl
{
    public class IdPoolImpl : IIdPool
    {
        private static long CURRENT_INDEX = 0;

        public long NextId()
        {
            return CURRENT_INDEX++;
        }
    }
}