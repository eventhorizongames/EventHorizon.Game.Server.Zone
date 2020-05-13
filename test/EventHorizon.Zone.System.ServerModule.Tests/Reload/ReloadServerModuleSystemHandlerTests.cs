namespace EventHorizon.Zone.System.ServerModule.Tests.Reload
{
    using EventHorizon.Zone.Core.Events.Client.Generic;
    using EventHorizon.Zone.System.ServerModule.Fetch;
    using EventHorizon.Zone.System.ServerModule.Load;
    using EventHorizon.Zone.System.ServerModule.Model;
    using EventHorizon.Zone.System.ServerModule.Model.Client;
    using EventHorizon.Zone.System.ServerModule.Reload;
    using global::System.Collections.Generic;
    using global::System.Threading;
    using global::System.Threading.Tasks;
    using MediatR;
    using Moq;
    using Xunit;

    public class ReloadServerModuleSystemHandlerTests
    {
        [Fact]
        public async Task ShouldPublishLoadServerModuleSystemEventWhenRequestIsHandled()
        {
            // Given
            var expected = new LoadServerModuleSystem();

            var mediatorMock = new Mock<IMediator>();

            // When
            var handler = new ReloadServerModuleSystemHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new ReloadServerModuleSystem(

                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    expected,
                    CancellationToken.None
                )
            );
        }

        [Fact]
        public async Task ShouldPublishClientActionToAllEventWhenRequestIsHandled()
        {
            // Given
            var serverModuleScriptsList = new List<ServerModuleScripts>();

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(
                mock => mock.Send(
                    new FetchServerModuleScriptList(),
                    CancellationToken.None
                )
            ).ReturnsAsync(
                serverModuleScriptsList
            );

            // When
            var handler = new ReloadServerModuleSystemHandler(
                mediatorMock.Object
            );
            await handler.Handle(
                new ReloadServerModuleSystem(

                ),
                CancellationToken.None
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish(
                    It.Is<ClientActionGenericToAllEvent>(
                        toAllEvent => toAllEvent.Data is ServerModuleSystemReloadedClientActionData
                    ),
                    CancellationToken.None
                )
            );
        }
    }
}
