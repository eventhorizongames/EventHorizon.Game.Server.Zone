namespace EventHorizon.Zone.System.ClientEntities.Tests.Query
{
    using EventHorizon.Zone.System.ClientEntities.Model;
    using EventHorizon.Zone.System.ClientEntities.Query;
    using EventHorizon.Zone.System.ClientEntities.State;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class QueryForAllRawClientEntityDetailsListHandlerTests
    {
        [Fact]
        public async Task ShouldReturnAllFromClientEntityRepositoryWhenReqeustIsHandled()
        {
            // Given
            var expected = new List<ClientEntity>();

            var clientEntityRepositoryMock = new Mock<ClientEntityRepository>();

            // When
            var handler = new QueryForAllRawClientEntityDetailsListHandler(
                clientEntityRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new QueryForAllRawClientEntityDetailsList(),
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
