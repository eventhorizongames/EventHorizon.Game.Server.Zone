namespace EventHorizon.Zone.System.ClientEntities.Tests.Query
{
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.ClientEntities.Query;
    using EventHorizon.Zone.System.ClientEntities.State;
    using Xunit;
    using global::System.Collections.Generic;
    using EventHorizon.Zone.System.ClientEntities.Model;
    using global::System.Threading;
    using Moq;

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