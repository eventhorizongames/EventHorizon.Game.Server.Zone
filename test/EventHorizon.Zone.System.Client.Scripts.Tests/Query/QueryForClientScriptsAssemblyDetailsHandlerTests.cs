namespace EventHorizon.Zone.System.Client.Scripts.Tests.Query
{
    using EventHorizon.Zone.System.Client.Scripts.Api;
    using EventHorizon.Zone.System.Client.Scripts.Events.Query;
    using EventHorizon.Zone.System.Client.Scripts.Model.Query;
    using EventHorizon.Zone.System.Client.Scripts.Query;

    using FluentAssertions;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class QueryForClientScriptsAssemblyDetailsHandlerTests
    {
        [Fact]
        public async Task ShouldReturnHashFromState()
        {
            // Given
            var hash = "hash";
            var expected = new ClientScriptsAssemblyDetails(
                hash
            );

            var clientScriptStateMock = new Mock<ClientScriptsState>();

            clientScriptStateMock.Setup(
                mock => mock.Hash
            ).Returns(
                hash
            );

            // When
            var handler = new QueryForClientScriptsAssemblyDetailsHandler(
                clientScriptStateMock.Object
            );
            var actual = await handler.Handle(
                new QueryForClientScriptsAssemblyDetails(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                expected
            );
        }
    }
}
