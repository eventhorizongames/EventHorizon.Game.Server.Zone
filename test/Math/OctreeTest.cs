using System;
using System.Numerics;
using EventHorizon.Game.Server.Zone.Math;
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

            octree.Add(new NodeEntity(Vector3.Zero));
            octree.Add(new NodeEntity(Vector3.Zero));
            octree.Add(new NodeEntity(Vector3.Zero));
            octree.Add(new NodeEntity(Vector3.Zero));
            octree.Add(new NodeEntity(Vector3.Zero));

            Assert.Equal(expectedAdded, octree.All().Count);
        }
        [Fact]
        public void TestAll_WhenAddAmountAboveTreeLevelShouldReturnExpectedAddedAmount()
        {
            var expectedAdded = 160;
            var octree = new Octree<NodeEntity>(Vector3.Zero, new Vector3(100), 0);

            for (int i = 0; i < 160; i++)
            {
                octree.Add(new NodeEntity(GetRandomPoint(100)));
            }

            Assert.Equal(expectedAdded, octree.All().Count);
        }

        Random random = new Random();
        private Vector3 GetRandomPoint(int v)
        {
            return new Vector3(random.Next(0, v), random.Next(0, v), random.Next(0, v));
        }

        public struct NodeEntity : IOctreeEntity
        {
            public Vector3 Position { get; private set; }

            public NodeEntity(Vector3 position)
            {
                Position = position;
            }
        }
    }
}