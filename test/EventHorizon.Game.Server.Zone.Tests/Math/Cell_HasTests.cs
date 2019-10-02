
using System.Numerics;
using EventHorizon.Game.Server.Zone.Tests.TestUtil;
using EventHorizon.Zone.Core.Model.Math;
using Xunit;
using static EventHorizon.Game.Server.Zone.Tests.Math.OctreeTest;

namespace EventHorizon.Game.Server.Zone.Tests.Math
{
    public class Cell_HasTests
    {
        [Fact]
        public void TestHas()
        {
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(10, 10, 10), 0);

            var point = new NodeEntity(new Vector3(3, 3, 3));
            cell.Add(point);

            Assert.True(cell.Has(point));
        }
        [Fact]
        public void TestHas_ShouldReturnFalseWhenInArea()
        {
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(10, 10, 10), 0);

            var point = new NodeEntity(new Vector3(3, 3, 3));

            Assert.False(cell.Has(point));
        }
        [Fact]
        public void TestHas_ShouldReturnTrueWhenInAccuracy()
        {
            var cell = new Cell<NodeEntity>(3, Vector3.Zero, new Vector3(10, 10, 10), 0);

            cell.Add(new NodeEntity(new Vector3(3, 3, 3)));
            var point = new NodeEntity(new Vector3(2, 2, 2));

            Assert.True(cell.Has(point));
        }
        [Fact]
        public void TestHas_ShouldReturnFalseWhenNotInAccuracy()
        {
            var cell = new Cell<NodeEntity>(2, Vector3.Zero, new Vector3(10, 10, 10), 0);

            cell.Add(new NodeEntity(new Vector3(3, 3, 3)));
            var point = new NodeEntity(new Vector3(1, 1, 1));

            Assert.False(cell.Has(point));
        }
        [Fact]
        public void TestHas_WhenContainsPointsShouldReturnTrueWhenInArea()
        {
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(10, 10, 10), 0);

            var point = new NodeEntity(new Vector3(3, 3, 3));
            cell.Add(point);
            var notPoint = new NodeEntity(new Vector3(3, 3, 3));

            Assert.True(cell.Has(notPoint));
        }
        [Fact]
        public void TestHas_WhenContainsPointsShouldReturnFalseWhenNotInArea()
        {
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(10, 10, 10), 0);

            var point = new NodeEntity(new Vector3(3, 3, 3));
            cell.Add(point);
            var notPoint = new NodeEntity(new Vector3(11, 11, 11));

            Assert.False(cell.Has(notPoint));
        }
        [Fact]
        public void TestHas_WhenContainsChildrenShouldReturnFalseWhenNotInArea()
        {
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(10, 10, 10), 0);
            for (int i = 0; i < 9; i++)
            {
                cell.Add(new NodeEntity(Vector3.Zero));
            }
            var notPoint = new NodeEntity(new Vector3(11, 11, 11));

            Assert.False(cell.Has(notPoint));
        }
        [Fact]
        public void TestHas_WhenContainsChildrenShouldReturnTrueWhenInArea()
        {
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(100, 100, 100), 0);
            for (int i = 0; i < 100; i++)
            {
                cell.Add(new NodeEntity(PointGenerator.GetRandomPoint(100)));
            }
            var point = PointGenerator.GetRandomPoint(10);
            cell.Add(new NodeEntity(point));

            Assert.True(cell.Has(new NodeEntity(point)));
        }
        [Fact]
        public void TestHas_WhenContainsLargeAmountOfChildrenShouldReturnTrueWhenInArea()
        {
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(100), 0);
            for (int i = 0; i < 100; i++)
            {
                cell.Add(new NodeEntity(PointGenerator.GetRandomPoint(100, 50)));
            }
            var lastPoint = PointGenerator.GetRandomPoint(25, 0);
            for (int i = 0; i < 9; i++)
            {
                lastPoint = PointGenerator.GetRandomPoint(25, 0);
                cell.Add(new NodeEntity(lastPoint));   
            }

            Assert.True(cell.Has(new NodeEntity(lastPoint)));
        }
    }
}