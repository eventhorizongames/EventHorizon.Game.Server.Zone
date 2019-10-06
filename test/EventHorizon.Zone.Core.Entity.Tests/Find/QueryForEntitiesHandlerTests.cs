using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Entity.Find;
using EventHorizon.Zone.Core.Events.Entity.Find;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.Core.Model.Entity.State;
using Moq;
using Xunit;

namespace EventHorizon.Zone.Core.Entity.Tests.Find
{
    public class QueryForEntitiesHandlerTests
    {
        [Fact]
        public async Task TestShouldReturnExpectedEntityListWhenCalledWithQuery()
        {
            // Given
            Func<IObjectEntity, bool> input = (entity) => true;
            var expected = new List<IObjectEntity>();

            var entityRepositoryMock = new Mock<EntityRepository>();
            entityRepositoryMock.Setup(
                mock => mock.Where(
                    input
                )
            ).ReturnsAsync(
                expected
            );

            // When
            var handler = new QueryForEntitiesHandler(
                entityRepositoryMock.Object
            );

            var actual = await handler.Handle(
                new QueryForEntities
                {
                    Query = input
                },
                CancellationToken.None
            );

            // Then
            Assert.Equal(
                expected,
                actual
            );
        }
    }
}