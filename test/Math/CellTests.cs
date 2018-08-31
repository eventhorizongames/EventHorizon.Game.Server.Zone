
using System.Numerics;
using EventHorizon.Game.Server.Zone.Math;
using Xunit;
using static EventHorizon.Game.Server.Zone.Tests.Math.OctreeTest;

namespace EventHorizon.Game.Server.Zone.Tests.Math
{
    public class CellTests
    {
        [Fact]
        public void TestHash()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, Vector3.Zero, 0);
            var cell = new Cell<NodeEntity>(octree, Vector3.Zero, new Vector3(10, 10, 10), 0);

            var point = new NodeEntity(new Vector3(3, 3, 3));
            cell.Add(point);

            Assert.True(cell.Has(point));
        }
        [Fact]
        public void TestHas_ShouldReturnFalseWhenInArea()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, Vector3.Zero, 0);
            var cell = new Cell<NodeEntity>(octree, Vector3.Zero, new Vector3(10, 10, 10), 0);

            var point = new NodeEntity(new Vector3(3, 3, 3));

            Assert.False(cell.Has(point));
        }
        [Fact]
        public void TestHas_WhenContainsPointsShouldReturnTrueWhenInArea()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, Vector3.Zero, 0);
            var cell = new Cell<NodeEntity>(octree, Vector3.Zero, new Vector3(10, 10, 10), 0);

            var point = new NodeEntity(new Vector3(3, 3, 3));
            cell.Add(point);
            var notPoint = new NodeEntity(new Vector3(3, 3, 3));

            Assert.True(cell.Has(notPoint));
        }
        [Fact]
        public void TestHas_WhenContainsPointsShouldReturnFalseWhenNotInArea()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, Vector3.Zero, 0);
            var cell = new Cell<NodeEntity>(octree, Vector3.Zero, new Vector3(10, 10, 10), 0);

            var point = new NodeEntity(new Vector3(3, 3, 3));
            cell.Add(point);
            var notPoint = new NodeEntity(new Vector3(11, 11, 11));

            Assert.False(cell.Has(notPoint));
        }
    }
}