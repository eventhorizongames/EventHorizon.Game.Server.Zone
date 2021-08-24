namespace EventHorizon.Game.Server.Zone.Tests.Math
{
    using System.Collections.Generic;
    using System.Numerics;

    using EventHorizon.Game.Server.Zone.Tests.TestUtil;
    using EventHorizon.Zone.Core.Model.Math;
    using EventHorizon.Zone.Core.Model.Structure;

    using FluentAssertions;

    using Xunit;

    public class OctreeTest
    {
        [Fact]
        public void TestCreate()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(10, 10, 10), 0);

            Assert.Equal(0, octree.Accuracy);
        }
        [Fact]
        public void TestAdd_ShouldAddExpected()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(10, 10, 10), 0);
            var expectedEntity = new NodeEntity(new Vector3(1));

            octree.Add(expectedEntity);

            Assert.True(octree.Has(expectedEntity));
        }
        [Fact]
        public void TestRemove_ShouldRemoveExpected()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(10, 10, 10), 0);
            var inputEntity = new NodeEntity(new Vector3(1));

            octree.Add(inputEntity);

            Assert.True(octree.Has(inputEntity));

            octree.Remove(inputEntity);

            Assert.False(octree.Has(inputEntity));
        }
        [Fact]
        public void TestAll_ShouldReturnExpectedAmountOfAdded()
        {
            var expectedAdded = 5;
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(10, 10, 10), 0);

            octree.Add(new NodeEntity(new Vector3(1)));
            octree.Add(new NodeEntity(new Vector3(2)));
            octree.Add(new NodeEntity(new Vector3(3)));
            octree.Add(new NodeEntity(new Vector3(4)));
            octree.Add(new NodeEntity(new Vector3(5)));

            Assert.Equal(expectedAdded, octree.All().Count);
        }
        [Fact]
        public void TestAll_WhenAddAmountAboveTreeLevelShouldReturnExpectedAddedAmount()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(100), 0);

            for (int i = 0; i < 160; i++)
            {
                octree.Add(new NodeEntity(PointGenerator.GetRandomPoint(100)));
            }

            Assert.NotEmpty(octree.All());
        }
        [Fact]
        public void TestFindNearestPoint()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(100), 0);

            for (int i = 0; i < 160; i++)
            {
                octree.Add(new NodeEntity(PointGenerator.GetRandomPoint(100)));
            }

            var expectedPoint = PointGenerator.GetRandomPoint(100);
            var expectedNodeEntity = new NodeEntity(expectedPoint);
            octree.Add(expectedNodeEntity);

            Assert.Equal(expectedNodeEntity, octree.FindNearestPoint(expectedPoint));
        }
        [Fact]
        public void TestFindNearestPoint_WithOptions()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(100), 0);

            for (int i = 0; i < 160; i++)
            {
                octree.Add(new NodeEntity(PointGenerator.GetRandomPoint(100)));
            }

            var expectedEntity = new NodeEntity(new Vector3(1, 1, 1));
            octree.Add(expectedEntity);
            var lookupPoint = new Vector3(1.5f, 1, 1);
            octree.Add(new NodeEntity(lookupPoint));

            var options = new IOctreeOptions(float.MaxValue, true);

            Assert.Equal(expectedEntity, octree.FindNearestPoint(lookupPoint, options));
        }
        [Fact]
        public void TestFindNearestPoint_NoPointFoundShouldReturnDefault()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(100), 0);

            Assert.Equal(default, octree.FindNearestPoint(Vector3.Zero));
        }
        [Fact]
        public void TestFindNearestPoint_ShouldReturnExpectedPointWhenContainsChildren()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(10), 0);

            var lookupPoint = new Vector3(1, 1, 1);
            var expectedPoint = new Vector3(3, 4, 3);
            var expectedEntity = new NodeEntity(expectedPoint);

            octree.Add(new NodeEntity(expectedPoint));
            octree.Add(new NodeEntity(new Vector3(3, 5, 3)));
            octree.Add(new NodeEntity(new Vector3(3, 6, 3)));

            octree.Add(new NodeEntity(new Vector3(5, 3, 5)));
            octree.Add(new NodeEntity(new Vector3(5, 4, 5)));
            octree.Add(new NodeEntity(new Vector3(5, 5, 5)));

            octree.Add(new NodeEntity(new Vector3(7, 7, 7)));
            octree.Add(new NodeEntity(new Vector3(7, 8, 7)));
            octree.Add(new NodeEntity(new Vector3(7, 9, 7)));

            Assert.Equal(expectedEntity, octree.FindNearestPoint(lookupPoint));
        }

        [Fact]
        public void TestFindNearbyPoints()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(10), 0);

            var lookupPoint = new Vector3(2);
            var expectedEntity1 = new NodeEntity(lookupPoint);
            octree.Add(expectedEntity1);

            var expectedEntity2 = new NodeEntity(Vector3.Subtract(lookupPoint, new Vector3(1)));
            octree.Add(expectedEntity2);

            var expectedEntity3 = new NodeEntity(Vector3.Add(lookupPoint, new Vector3(1)));
            octree.Add(expectedEntity3);

            octree.Add(new NodeEntity(new Vector3(3, 5, 3)));
            octree.Add(new NodeEntity(new Vector3(3, 6, 3)));

            octree.Add(new NodeEntity(new Vector3(5, 3, 5)));
            octree.Add(new NodeEntity(new Vector3(5, 4, 5)));
            octree.Add(new NodeEntity(new Vector3(5, 5, 5)));

            octree.Add(new NodeEntity(new Vector3(7, 7, 7)));
            octree.Add(new NodeEntity(new Vector3(7, 8, 7)));
            octree.Add(new NodeEntity(new Vector3(7, 9, 7)));

            var list = octree.FindNearbyPoints(lookupPoint, 2);

            list.Should().BeEquivalentTo(
                new List<NodeEntity>
                {
                    expectedEntity2,
                    expectedEntity1,
                    expectedEntity3
                }
            );
        }
        [Fact]
        public void TestFindNearbyPoints_WithOptions()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(10), 0);

            var lookupPoint = new Vector3(2);
            var expectedEntity1 = new NodeEntity(lookupPoint);
            octree.Add(expectedEntity1);

            var expectedEntity2 = new NodeEntity(Vector3.Subtract(lookupPoint, new Vector3(1)));
            octree.Add(expectedEntity2);

            var expectedEntity3 = new NodeEntity(Vector3.Add(lookupPoint, new Vector3(1)));
            octree.Add(expectedEntity3);

            octree.Add(new NodeEntity(new Vector3(3, 5, 3)));
            octree.Add(new NodeEntity(new Vector3(3, 6, 3)));

            octree.Add(new NodeEntity(new Vector3(5, 3, 5)));
            octree.Add(new NodeEntity(new Vector3(5, 4, 5)));
            octree.Add(new NodeEntity(new Vector3(5, 5, 5)));

            octree.Add(new NodeEntity(new Vector3(7, 7, 7)));
            octree.Add(new NodeEntity(new Vector3(7, 8, 7)));
            octree.Add(new NodeEntity(new Vector3(7, 9, 7)));

            var options = new IOctreeOptions(float.MaxValue, true);
            var list = octree.FindNearbyPoints(lookupPoint, 2, options);

            list.Should().BeEquivalentTo(
                new List<NodeEntity>
                {
                    expectedEntity2,
                    expectedEntity3,
                }
            );
        }
        [Fact]
        public void TestFindNearbyPoints_ShouldReturnExpectedPointsWhenContainsChildren()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(10), 0);

            var lookupPoint = new Vector3(1, 1, 1);
            var expectedPoint = new Vector3(3, 4, 3);
            var expectedEntity = new NodeEntity(expectedPoint);

            octree.Add(new NodeEntity(expectedPoint));
            octree.Add(new NodeEntity(new Vector3(3, 5, 3)));
            octree.Add(new NodeEntity(new Vector3(3, 6, 3)));

            octree.Add(new NodeEntity(new Vector3(5, 3, 5)));
            octree.Add(new NodeEntity(new Vector3(5, 4, 5)));
            octree.Add(new NodeEntity(new Vector3(5, 5, 5)));

            octree.Add(new NodeEntity(new Vector3(7, 7, 7)));
            octree.Add(new NodeEntity(new Vector3(7, 8, 7)));
            octree.Add(new NodeEntity(new Vector3(7, 9, 7)));

            Assert.Equal(expectedEntity, octree.FindNearestPoint(lookupPoint));
        }

        public struct NodeEntity : IOctreeEntity
        {
            public Vector3 Position { get; private set; }

            public NodeEntity(Vector3 position)
            {
                Position = position;
            }
            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                return Position.Equals(((NodeEntity)obj).Position);
            }

            public override int GetHashCode()
            {
                return Position.GetHashCode();
            }
            public override string ToString()
            {
                return Position.ToString();
            }

            public static bool operator ==(NodeEntity left, NodeEntity right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(NodeEntity left, NodeEntity right)
            {
                return !(left == right);
            }
        }
    }
}
