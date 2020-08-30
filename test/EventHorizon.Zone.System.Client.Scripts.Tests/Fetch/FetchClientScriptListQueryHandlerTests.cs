namespace EventHorizon.Zone.System.Client.Scripts.Tests.Fetch
{
    using global::System.Collections.Generic;
    using global::System.Threading;
    using EventHorizon.Zone.System.Client.Scripts.Events.Fetch;
    using EventHorizon.Zone.System.Client.Scripts.Fetch;
    using EventHorizon.Zone.System.Client.Scripts.Model;
    using global::System.Threading.Tasks;
    using Moq;
    using Xunit;
    using FluentAssertions;
    using EventHorizon.Zone.System.Client.Scripts.Api;

    public class FetchClientScriptListQueryHandlerTests
    {
        [Fact]
        public async Task ShouldReturnAllJavaScriptScriptsFromRepository()
        {
            // Given
            var expected = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.JavaScript,
                    "path",
                    "file-name",
                    "script-string"
                ),
            };
            var repositoryAll = new List<ClientScript>
            {
                ClientScript.Create(
                    ClientScriptType.JavaScript,
                    "path",
                    "file-name",
                    "script-string"
                ),
                ClientScript.Create(
                    ClientScriptType.Unknown,
                    "path",
                    "file-name",
                    "script-string"
                ),
                ClientScript.Create(
                    ClientScriptType.CSharp,
                    "path",
                    "file-name",
                    "script-string"
                ),
            };

            var clientScriptRepositoryMock = new Mock<ClientScriptRepository>();

            clientScriptRepositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                repositoryAll
            );

            // When
            var handler = new FetchClientScriptListQueryHandler(
                clientScriptRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new FetchClientScriptListQuery(),
                CancellationToken.None
            );

            // Then
            actual.Should().BeEquivalentTo(
                expected
            );
        }
    }
}
