using Xunit;
using Moq;
using EventHorizon.Game.Server.Zone.Entity.Model;
using EventHorizon.Game.Server.Zone.Core.Model;
using System.Collections.Generic;
using EventHorizon.Game.Server.Zone.Model.Entity;
using EventHorizon.Game.Server.Zone.Model.Core;
using System;

namespace EventHorizon.Game.Server.Zone.Tests.TestUtil.Models
{
    public struct TestObjectEntity : IObjectEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string GlobalId { get; set; }

        public EntityType Type { get { return EntityType.OTHER; } }
        public PositionState Position { get; set; }
        public IList<string> TagList { get; set; }

        private Dictionary<string, object> _data;
        private Dictionary<string, object> _rawData;
        public Dictionary<string, object> RawData
        {
            get
            {
                return _rawData ?? new Dictionary<string, object>();
            }
            set
            {
                _data = new Dictionary<string, object>();
                _rawData = value;
            }
        }
        public Dictionary<string, object> Data
        {
            get
            {
                return _data ?? new Dictionary<string, object>();
            }
        }

        public bool IsFound()
        {
            return !this.Equals(default(TestObjectEntity));
        }
    }
}