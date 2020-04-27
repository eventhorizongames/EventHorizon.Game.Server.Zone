namespace EventHorizon.Zone.System.ClientEntities.Tests.Query
{
    using EventHorizon.Zone.Core.Model.Entity;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.Query;
    using EventHorizon.Zone.System.ClientEntities.State;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using Moq;
    using Xunit;

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