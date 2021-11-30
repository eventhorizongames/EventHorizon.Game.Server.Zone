namespace EventHorizon.BackgroundTasks.Tests.HostedService;

using System;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using EventHorizon.BackgroundTasks.HostedService;
using EventHorizon.BackgroundTasks.Model;
using EventHorizon.Test.Common.Attributes;
using EventHorizon.Test.Common.Utils;

using MediatR;

using Moq;

using Xunit;


public class BackgroundTasksHostedServiceTests
{
    [Theory, AutoMoqData]
    public async Task TriggersSendTaskWhenBackgroundJobsAvailable(
        [Frozen] BackgroundTask backgroundTask,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IServiceProvider> serviceProviderMock,
        [Frozen] ServiceScopeFactoryMock serviceScopeFactoryMock,
        BackgroundTasksHostedService backgroundTasksHostedService
    )
    {
        // Given
        serviceScopeFactoryMock.WithMock(
            senderMock
        );
        serviceScopeFactoryMock.WithMock(
            serviceScopeFactoryMock
        );
        serviceScopeFactoryMock.WithMock(
            serviceProviderMock
        );

        // When
        var cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromMilliseconds(1)
        );
        var cancellationToken = cancellationTokenSource.Token;
        await backgroundTasksHostedService.StartAsync(
            cancellationToken
        );

        // Then
        senderMock.Verify(
            mock => mock.Send(
                backgroundTask,
                It.IsAny<CancellationToken>()
            )
        );
        _ = backgroundTasksHostedService.StopAsync(
            cancellationToken
        );
    }

    [Theory, AutoMoqData]
    public async Task IgnoreBackgroundJobsProcessingWhenExceptionIsThrown(
        [Frozen] BackgroundTask backgroundTask,
        [Frozen] Mock<ISender> senderMock,
        [Frozen] Mock<IServiceProvider> serviceProviderMock,
        [Frozen] ServiceScopeFactoryMock serviceScopeFactoryMock,
        BackgroundTasksHostedService backgroundTasksHostedService
    )
    {
        // Given
        serviceScopeFactoryMock.WithMock(
            senderMock
        );
        serviceScopeFactoryMock.WithMock(
            serviceScopeFactoryMock
        );
        serviceScopeFactoryMock.WithMock(
            serviceProviderMock
        );

        serviceProviderMock.Setup(
            mock => mock.GetService(typeof(ISender))
        ).Throws(
            new Exception("error")
        );

        // When
        var cancellationTokenSource = new CancellationTokenSource(
            TimeSpan.FromMilliseconds(1)
        );
        var cancellationToken = cancellationTokenSource.Token;
        await backgroundTasksHostedService.StartAsync(
            cancellationToken
        );

        // Then
        senderMock.Verify(
            mock => mock.Send(
                backgroundTask,
                It.IsAny<CancellationToken>()
            ),
            Times.Never()
        );
        _ = backgroundTasksHostedService.StopAsync(
            cancellationToken
        );
    }
}
