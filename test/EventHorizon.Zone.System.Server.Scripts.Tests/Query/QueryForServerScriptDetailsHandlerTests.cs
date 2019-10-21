using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Zone.System.Server.Scripts.Events.Query;
using EventHorizon.Zone.System.Server.Scripts.Model.Details;
using EventHorizon.Zone.System.Server.Scripts.Query;
using EventHorizon.Zone.System.Server.Scripts.State;
using Moq;
using Xunit;

namespace EventHorizon.Zone.System.Server.Scripts.Tests.Query
{
    public class QueryForServerScriptDetailsHandlerTests
    {
        [Fact]
        public async Task TestShouldReturnServerScriptDetailsWheRepositoryIsWhereIsCalled()
        {
            // Given
            var expectedDetails = new ServerScriptDetails(
                "id",
                "file-name",
                "path",
                "script-string",
                null,
                null,
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
            Assert.Collection(
                actual,
                actualDetails => Assert.Equal(
                    expectedDetails, 
                    actualDetails
                )
            );
        }
    }
}