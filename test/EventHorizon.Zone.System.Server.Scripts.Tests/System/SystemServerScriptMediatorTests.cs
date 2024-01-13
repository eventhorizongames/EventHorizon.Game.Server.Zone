namespace EventHorizon.Zone.System.Server.Scripts.Tests.System;


using AutoFixture.Xunit2;

using EventHorizon.Test.Common.Attributes;
using EventHorizon.Zone.System.Server.Scripts.System;

using global::System;
using global::System.Threading;
using global::System.Threading.Tasks;

using MediatR;

using Moq;

using Xunit;

public class SystemServerScriptMediatorTests
{
    [Theory, AutoMoqData]
    public async Task ShouldDelegateToMediatorWhenPublishingNotification(
        // Given
        TestingNotification notification,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<IServiceProvider> serviceProviderMock,
        SystemServerScriptMediator scriptMediator
    )
    {
        serviceProviderMock.Setup(
            mock => mock.GetService(typeof(IMediator))
        ).Returns(
            mediatorMock.Object
        );

        // When
        await scriptMediator.Publish(
            notification,
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Publish(
                notification,
                CancellationToken.None
            )
        );
    }

    [Theory, AutoMoqData]
    public async Task ShouldDelegateToMediatorWhenSendingReqeust(
        // Given
        TestingRequest request,
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] Mock<IServiceProvider> serviceProviderMock,
        SystemServerScriptMediator scriptMediator
    )
    {
        serviceProviderMock.Setup(
            mock => mock.GetService(typeof(IMediator))
        ).Returns(
            mediatorMock.Object
        );

        // When
        await scriptMediator.Send(
            request,
            CancellationToken.None
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Send(
                request,
                CancellationToken.None
            )
        );
    }

    public class TestingNotification
        : INotification
    {

    }

    public class TestingRequest
        : IRequest<string>
    {

    }
}
