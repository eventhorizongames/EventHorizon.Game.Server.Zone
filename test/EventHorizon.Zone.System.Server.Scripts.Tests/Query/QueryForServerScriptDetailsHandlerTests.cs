namespace EventHorizon.Zone.System.Server.Scripts.Tests.Query
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using EventHorizon.Zone.System.Server.Scripts.Events.Query;
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Query;
    using EventHorizon.Zone.System.Server.Scripts.State;
    using Moq;
    using Xunit;
    using FluentAssertions;

    public class QueryForServerScriptDetailsHandlerTests
    {
        [Fact]
        public async Task TestShouldReturnServerScriptDetailsWheRepositoryIsWhereIsCalled()
        {
            // Given
            var expectedDetails = new ServerScriptDetails(
                "id",
                "hash",
                "file-name",
                "path",
                "script-string",
                null
            );
            Func<ServerScriptDetails, bool> query = details => true;

            var serverScriptDetailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();

            serverScriptDetailsRepositoryMock.Setup(
                mock => mock.Where(
                    query
                )
            ).Returns(
                new List<ServerScriptDetails>
                {
                    expectedDetails
                }
            );

            // When
            var handler = new QueryForServerScriptDetailsHandler(
                serverScriptDetailsRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new QueryForServerScriptDetails(
                    query
                ),
                CancellationToken.None
            );

            // Then
            actual.Should().BeEquivalentTo(
                new List<ServerScriptDetails>
                {
                    expectedDetails
                }
            );
        }
    }
}