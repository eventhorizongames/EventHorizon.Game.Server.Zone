namespace EventHorizon.Zone.System.Server.Scripts.Tests.Set
{
    using EventHorizon.Zone.System.Server.Scripts.Model.Details;
    using EventHorizon.Zone.System.Server.Scripts.Set;
    using EventHorizon.Zone.System.Server.Scripts.State;

    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Moq;

    using Xunit;


    public class SetServerScriptDetailsCommandHandlerTests
    {
        [Fact]
        public async Task ShouldAddedScriptToRepositoryWhenCommandIsHandled()
        {
            // Given
            var scriptId = "script-id";
            var expected = new ServerScriptDetails(
                scriptId,
                null,
                null,
                null,
                null,
                null
            );

            var serverScriptDetailsRepositoryMock = new Mock<ServerScriptDetailsRepository>();

            // When
            var handler = new SetServerScriptDetailsCommandHandler(
                serverScriptDetailsRepositoryMock.Object
            );
            var actual = await handler.Handle(
                new SetServerScriptDetailsCommand(
                    expected
                ),
                CancellationToken.None
            );

            // Then
            serverScriptDetailsRepositoryMock.Verify(
                mock => mock.Add(
                    scriptId,
                    expected
                )
            );

        }
    }
}
