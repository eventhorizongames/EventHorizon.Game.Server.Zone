using System;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Math;
using EventHorizon.Game.Server.Zone.Tests.TestUtil;
using Xunit;

namespace EventHorizon.Game.Server.Zone.Tests.Math
{
    public class OctreeTest
    {
        [Fact]
        public void TestCreate()
        {
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(10, 10, 10), 0);

            Assert.Equal(0, octree.Accuracy);
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
            var expectedAdded = 160;
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(100), 0);

            for (int i = 0; i < 160; i++)
            {
                octree.Add(new NodeEntity(PointGenerator.GetRandomPoint(100)));
            }

            Assert.Equal(expectedAdded, octree.All().Count);
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
        }
    }
}