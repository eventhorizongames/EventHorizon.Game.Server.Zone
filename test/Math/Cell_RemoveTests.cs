using System.Numerics;
using EventHorizon.Game.Server.Zone.Math;
using EventHorizon.Game.Server.Zone.Tests.TestUtil;
using Xunit;
using static EventHorizon.Game.Server.Zone.Tests.Math.OctreeTest;

namespace EventHorizon.Game.Server.Zone.Tests.Math
{
    public class Cell_RemoveTests
    {
        [Fact]
        public void TestRemove()
        {
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(10, 10, 10), 0);

            cell.Add(new NodeEntity(new Vector3(3)));

            Assert.NotEmpty(cell.Points);

            cell.Remove(new NodeEntity(new Vector3(3)));
            
            Assert.Empty(cell.Points);
        }
        [Fact]
        public void TestRemove_ShouldRemove()
        {
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(10, 10, 10), 0);

            cell.Add(new NodeEntity(PointGenerator.GetRandomPoint(9, 9)));
            
            var pointToRemove = new Vector3(1);
            cell.Add(new NodeEntity(pointToRemove));

            Assert.True(cell.Has(new NodeEntity(pointToRemove)));

            cell.Remove(new NodeEntity(pointToRemove));

            Assert.False(cell.Has(new NodeEntity(pointToRemove)));
        }
    }
}