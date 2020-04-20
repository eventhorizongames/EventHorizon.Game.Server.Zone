using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.Core.Model.Entity;
using EventHorizon.Zone.System.ClientEntities.Model;
using EventHorizon.Zone.System.ClientEntities.Query;
using EventHorizon.Zone.System.ClientEntities.State;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.ClientEntities.Tests.Query
{
    public class QueryForAllClientEntityDetailsListHandlerTests
    {
        [Fact]
        public async Task ShouldReturnAllFromClientEntityRepositoryWhenReqeustIsHandled()
        {
            // Given
            var result = new List<ClientEntity>();
            var expected = result.Cast<IObjectEntity>();

            var clientEntityRepositoryMock = new Mock<ClientEntityRepository>();

            clientEntityRepositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                result
            );

            // When
            var handler = new QueryForAllClientEntityDetailsListHandler(
                clientEntityRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new QueryForAllClientEntityDetailsList(),
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