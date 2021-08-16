namespace EventHorizon.Zone.System.ServerModule.Tests.Fetch
{
    using EventHorizon.Zone.System.ServerModule.Fetch;
    using EventHorizon.Zone.System.ServerModule.Model;
    using EventHorizon.Zone.System.ServerModule.State;

    using FluentAssertions;

    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class FetchServerModuleListHandlerTests
    {
        [Fact]
        public async Task ShouldReturnListOfAllServerModulesFromServerModuleRespository()
        {
            // Given
            var expected = new List<ServerModuleScripts>
            {
                new ServerModuleScripts()
            };

            var serverModuleRepositoryMock = new Mock<ServerModuleRepository>();

            serverModuleRepositoryMock.Setup(
                mock => mock.All()
            ).Returns(
                expected
            );

            // When
            var handler = new FetchServerModuleScriptListHandler(
                serverModuleRepositoryMock.Object
            );

            var actual = await handler.Handle(
                new FetchServerModuleScriptList(),
                CancellationToken.None
            );

            // Then
            actual
                .Should().BeEquivalentTo(expected);

        }
    }
}
