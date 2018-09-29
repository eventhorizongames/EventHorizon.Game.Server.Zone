using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Core.Model;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Model.Core;

namespace EventHorizon.Game.Server.Zone.Tests.TestUtil.Models
{
    public struct TestObjectEntity : IObjectEntity
    {
        public long Id { get; set; }

        public EntityType Type { get { return EntityType.OTHER; } }
        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }
        public Dictionary<string, object> Data { get; set; }

        public T GetProperty<T>(string prop)
        {
            return (T)Data[prop];
        }

        public bool IsFound()
        {
            return !this.Equals(default(TestObjectEntity));
        }
    }
}