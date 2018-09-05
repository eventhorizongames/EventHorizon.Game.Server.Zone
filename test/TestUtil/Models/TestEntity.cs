using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Core.Model;

namespace EventHorizon.Game.Server.Zone.Tests.TestUtil.Models
{
    public struct TestObjectEntity : IObjectEntity
    {
        public long Id { get; set; }

        public EntityType Type { get { return EntityType.OTHER; } }

        public PositionState Position { get; set; }
        public dynamic Data { get; set; }

        public bool IsFound()
        {
            return !this.Equals(default(TestObjectEntity));
        }
    }
}