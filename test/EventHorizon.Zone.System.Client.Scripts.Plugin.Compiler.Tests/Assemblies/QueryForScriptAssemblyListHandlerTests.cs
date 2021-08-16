namespace EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Tests.Assemblies
{
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.System.Client.Scripts.Plugin.Compiler.Assemblies;

    using FluentAssertions;

    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;

    public class QueryForScriptAssemblyListHandlerTests
    {
        [Fact]
        public async Task ShouldReturnListOfAssembliesForOnlyEventHorizonWhenEventIsHandled()
        {
            // Given
            var expectedToContain = "EventHorizon";

            var serverInfoMock = new Mock<ServerInfo>();

            serverInfoMock.Setup(
                mock => mock.AssembliesPath
            ).Returns(
                AppDomain.CurrentDomain.BaseDirectory
            );

            // When
            var handler = new QueryForScriptAssemblyListHandler(
                serverInfoMock.Object
            );
            var actual = await handler.Handle(
                new QueryForScriptAssemblyList(

                ),
                CancellationToken.None
            );

            // Then
            actual.Should().NotBeEmpty();

            foreach (var assemblyFound in actual)
            {
                assemblyFound.FullName
                    .Should()
                    .Contain(
                        expectedToContain
                    );
            }

        }
    }
}
