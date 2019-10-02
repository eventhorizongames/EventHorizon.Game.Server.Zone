using System.Collections.Generic;
using System.Numerics;
using EventHorizon.Zone.Core.Model.Math;
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

            Assert.NotEmpty(cell.Search_Points);

            cell.Remove(new NodeEntity(new Vector3(3)));

            Assert.Empty(cell.Search_Points);
        }
        [Fact]
        public void TestRemove_ShouldRemoveWhenCellHasChildren()
        {
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(10, 10, 10), 0);

            cell.Add(new NodeEntity(new Vector3(3, 4, 3)));
            cell.Add(new NodeEntity(new Vector3(3, 5, 3)));
            cell.Add(new NodeEntity(new Vector3(3, 6, 3)));

            cell.Add(new NodeEntity(new Vector3(5, 3, 5)));
            cell.Add(new NodeEntity(new Vector3(5, 4, 5)));
            cell.Add(new NodeEntity(new Vector3(5, 5, 5)));

            cell.Add(new NodeEntity(new Vector3(7, 7, 7)));
            cell.Add(new NodeEntity(new Vector3(7, 8, 7)));
            cell.Add(new NodeEntity(new Vector3(7, 9, 7)));

            Assert.Collection(cell.Search_Children,
                child => Assert.NotEmpty(child.Search_Points),
                child => Assert.Empty(child.Search_Points),
                child => Assert.Empty(child.Search_Points),
                child => Assert.NotEmpty(child.Search_Points),
                child => Assert.NotEmpty(child.Search_Points),
                child => Assert.Empty(child.Search_Points),
                child => Assert.Empty(child.Search_Points),
                child => Assert.NotEmpty(child.Search_Points));

            var pointToRemove = new Vector3(1);
            cell.Add(new NodeEntity(pointToRemove));
            Assert.True(cell.Has(new NodeEntity(pointToRemove)));

            cell.Remove(new NodeEntity(pointToRemove));

            Assert.False(cell.Has(new NodeEntity(pointToRemove)));
        }
        [Fact]
        public void TestRemove_ShouldMergePointsIntoCellWhenCellHasChildrenAndGoWithInLimit()
        {
            var positionToRemove = new Vector3(7, 9, 7);
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(10, 10, 10), 0);

            cell.Add(new NodeEntity(new Vector3(3, 4, 3)));
            cell.Add(new NodeEntity(new Vector3(3, 5, 3)));
            cell.Add(new NodeEntity(new Vector3(3, 6, 3)));

            cell.Add(new NodeEntity(new Vector3(5, 3, 5)));
            cell.Add(new NodeEntity(new Vector3(5, 4, 5)));
            cell.Add(new NodeEntity(new Vector3(5, 5, 5)));

            cell.Add(new NodeEntity(new Vector3(7, 7, 7)));
            cell.Add(new NodeEntity(new Vector3(7, 8, 7)));
            cell.Add(new NodeEntity(positionToRemove));

            Assert.Collection(cell.Search_Children,
                child => Assert.NotEmpty(child.Search_Points),
                child => Assert.Empty(child.Search_Points),
                child => Assert.Empty(child.Search_Points),
                child => Assert.NotEmpty(child.Search_Points),
                child => Assert.NotEmpty(child.Search_Points),
                child => Assert.Empty(child.Search_Points),
                child => Assert.Empty(child.Search_Points),
                child => Assert.NotEmpty(child.Search_Points));

            cell.Remove(new NodeEntity(positionToRemove));

            Assert.Empty(cell.Search_Children);
            Assert.NotEmpty(cell.Search_Points);
        }
        [Fact]
        public void TestRemove_ShouldNotMergePointsIntoCellWhenAChildHasChildren()
        {
            var positionToRemove = new Vector3(7, 9, 7);
            var cell = new Cell<NodeEntity>(0, Vector3.Zero, new Vector3(100, 100, 100), 0);

            cell.Add(new NodeEntity(new Vector3(3, 4, 3)));
            cell.Add(new NodeEntity(new Vector3(3, 5, 3)));
            cell.Add(new NodeEntity(new Vector3(3, 6, 3)));

            cell.Add(new NodeEntity(new Vector3(5, 3, 5)));
            cell.Add(new NodeEntity(new Vector3(5, 4, 5)));
            cell.Add(new NodeEntity(new Vector3(5, 5, 5)));

            cell.Add(new NodeEntity(new Vector3(7, 7, 7)));
            cell.Add(new NodeEntity(new Vector3(7, 8, 7)));
            cell.Add(new NodeEntity(positionToRemove));

            cell.Add(new NodeEntity(new Vector3(8, 8, 8)));

            Assert.Collection(cell.Search_Children,
                ValidateEmptyNode,
                ValidateEmptyNode,
                ValidateEmptyNode,
                ValidateEmptyNode,
                ValidateEmptyNode,
                ValidateEmptyNode,
                ValidateEmptyNode,
                child =>
                {
                    Assert.NotEmpty(child.Search_Children);
                    Assert.Collection(child.Search_Children,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        childChild =>
                        {
                            Assert.NotEmpty(childChild.Search_Children);
                            Assert.Collection(childChild.Search_Children,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                childChildChild =>
                                {
                                    Assert.NotEmpty(childChildChild.Search_Children);
                                    Assert.Collection(childChildChild.Search_Children,
                                        childChildChildChild =>
                                        {
                                            Assert.Empty(childChildChildChild.Search_Children);
                                            Assert.NotEmpty(childChildChildChild.Search_Points);
                                            Assert.Collection(childChildChildChild.Search_Points,
                                                point => Assert.Equal(new NodeEntity(new Vector3(7, 9, 7)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(7, 7, 7)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(7, 8, 7)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(8, 8, 8)), point.Value)
                                            );
                                        },
                                        ValidateEmptyNode,
                                        ValidateEmptyNode,
                                        ValidateEmptyNode,
                                        ValidateEmptyNode,
                                        ValidateEmptyNode,
                                        ValidateEmptyNode,
                                        childChildChildChild =>
                                        {
                                            Assert.Empty(childChildChildChild.Search_Children);
                                            Assert.NotEmpty(childChildChildChild.Search_Points);
                                            Assert.Collection(childChildChildChild.Search_Points,
                                                point => Assert.Equal(new NodeEntity(new Vector3(3, 6, 3)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(3, 4, 3)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(3, 5, 3)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(5, 3, 5)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(5, 5, 5)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(5, 4, 5)), point.Value)
                                            );
                                        }
                                    );
                                }
                            );
                        }
                    );
                });

            cell.Remove(new NodeEntity(positionToRemove));

            Assert.Collection(cell.Search_Children,
                ValidateEmptyNode,
                ValidateEmptyNode,
                ValidateEmptyNode,
                ValidateEmptyNode,
                ValidateEmptyNode,
                ValidateEmptyNode,
                ValidateEmptyNode,
                child =>
                {
                    Assert.NotEmpty(child.Search_Children);
                    Assert.Collection(child.Search_Children,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        ValidateEmptyNode,
                        childChild =>
                        {
                            Assert.NotEmpty(childChild.Search_Children);
                            Assert.Collection(childChild.Search_Children,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                ValidateEmptyNode,
                                childChildChild =>
                                {
                                    Assert.NotEmpty(childChildChild.Search_Children);
                                    Assert.Collection(childChildChild.Search_Children,
                                        childChildChildChild =>
                                        {
                                            Assert.Empty(childChildChildChild.Search_Children);
                                            Assert.NotEmpty(childChildChildChild.Search_Points);
                                            Assert.Collection(childChildChildChild.Search_Points,
                                                point => Assert.Equal(new NodeEntity(new Vector3(7, 7, 7)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(7, 8, 7)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(8, 8, 8)), point.Value)
                                            );
                                        },
                                        ValidateEmptyNode,
                                        ValidateEmptyNode,
                                        ValidateEmptyNode,
                                        ValidateEmptyNode,
                                        ValidateEmptyNode,
                                        ValidateEmptyNode,
                                        childChildChildChild =>
                                        {
                                            Assert.Empty(childChildChildChild.Search_Children);
                                            Assert.NotEmpty(childChildChildChild.Search_Points);
                                            Assert.Collection(childChildChildChild.Search_Points,
                                                point => Assert.Equal(new NodeEntity(new Vector3(3, 6, 3)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(3, 4, 3)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(3, 5, 3)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(5, 3, 5)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(5, 5, 5)), point.Value),
                                                point => Assert.Equal(new NodeEntity(new Vector3(5, 4, 5)), point.Value)
                                            );
                                        }
                                    );
                                }
                            );
                        }
                    );
                });
        }

        private void ValidateEmptyNode(Cell<NodeEntity> cell)
        {
            Assert.Empty(cell.Search_Children);
            Assert.Empty(cell.Search_Points);
        }
    }
}