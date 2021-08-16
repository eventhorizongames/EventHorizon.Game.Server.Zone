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

    public class QueryForClientScriptsAssemblyHandlerTests
    {
        [Fact]
        public async Task ShouldReturnHashAndScriptAssemblyFromState()
        {
            // Given
            var hash = "hash";
            var scriptAssembly = "script-assembly";
            var expected = new ClientScriptsAssemblyResult(
                hash,
                scriptAssembly
            );

            var clientScriptsStateMock = new Mock<ClientScriptsState>();

            clientScriptsStateMock.Setup(
                mock => mock.Hash
            ).Returns(
                hash
            );

            clientScriptsStateMock.Setup(
                mock => mock.ScriptAssembly
            ).Returns(
                scriptAssembly
            );

            // When
            var handler = new QueryForClientScriptsAssemblyHandler(
                clientScriptsStateMock.Object
            );
            var actual = await handler.Handle(
                new QueryForClientScriptsAssembly(),
                CancellationToken.None
            );

            // Then
            actual.Should().Be(
                expected
            );
        }
    }
}
