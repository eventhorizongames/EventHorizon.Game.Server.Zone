namespace EventHorizon.Zone.System.EntityModule.Tests.Reload;

using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.Core.Events.Client.Generic;
using EventHorizon.Zone.System.EntityModule.Api;
using EventHorizon.Zone.System.EntityModule.Load;
using EventHorizon.Zone.System.EntityModule.Reload;

using FluentAssertions;

using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class ReloadEntityModuleSystemCommandHandlerTests
{
    [Theory, AutoMoqData]
    public async Task ReturnSuccessfulResultWhenCommandIsHandledWithoutErrors(
        // Given
        ReloadEntityModuleSystemCommandHandler handler
    )
    {
        // When
        var actual = await handler.Handle(
            new ReloadEntityModuleSystemCommand(),
            CancellationToken.None
        );

        // Then
        actual.Success.Should().BeTrue();
    }

    [Theory, AutoMoqData]
    public async Task ClearsAndLoadsNewSystemAndPublishReloadedEventWhenCommandIsHandled(
        // Given
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IPublisher> publisherMock,
        [Frozen] Mock<EntityModuleRepository> entityModuleRepositoryMock,
        ReloadEntityModuleSystemCommandHandler handler
    )
    {
        var expectedClientAction = "ENTITY_MODULE_SYSTEM_RELOADED_CLIENT_ACTION_EVENT";

        // When
        var actual = await handler.Handle(
            new ReloadEntityModuleSystemCommand(),
            CancellationToken.None
        );

        // Then
        entityModuleRepositoryMock.Verify(
            mock => mock.Clear()
        );
        senderMock.Verify(
            mock => mock.Send(
                new LoadEntityModuleSystemCommand(),
                CancellationToken.None
            )
        );
        publisherMock.Verify(
            mock => mock.Publish(
                It.Is<ClientActionGenericToAllEvent>(
                    a => a.Action == expectedClientAction
                ),
                CancellationToken.None
            )
        );
    }
}
