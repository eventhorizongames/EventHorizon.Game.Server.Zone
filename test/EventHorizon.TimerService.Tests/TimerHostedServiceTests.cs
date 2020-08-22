namespace EventHorizon.TimerService.Tests.TimerService
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Test.Common.Utils;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class TimerHostedServiceTests
    {
        [Fact]
        public async Task TestExecuteAsync_ShouldStartAllTimerTasksPassedIn()
        {
            // Given
            var inputCancellationTokenSource = new CancellationTokenSource();

            var inputPeriod = 100;
            var expectedPublishedEvent = new TestNotificationEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();
            var timerTaskMock = new Mock<ITimerTask>();

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            var timerTasksMock = new List<ITimerTask>();
            
            var serviceScopeMocks = ServiceScopeFactoryUtils.SetupServiceScopeFactoryWithMediatorMock(
                mediatorMock
            );

            timerTaskMock.Setup(
                mock => mock.OnRunEvent
            ).Returns(
                expectedPublishedEvent
            );
            timerTaskMock.Setup(
                mock => mock.Period
            ).Returns(
                inputPeriod
            );
            timerTasksMock.Add(
                timerTaskMock.Object
            );

            // When
            var timerHostedService = new TimerHostedService(
                loggerFactoryMock.Object,
                serviceScopeMocks.Object,
                timerTasksMock
            );

            await timerHostedService.StartAsync(
                inputCancellationTokenSource.Token
            );

            Thread.Sleep(125);

            // Then
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    It.IsAny<INotification>(), 
                    CancellationToken.None
                ), 
                Times.AtLeastOnce()
            );
            
            await timerHostedService.StopAsync(
                inputCancellationTokenSource.Token
            );
        }
    }

    internal class TestNotificationEvent : INotification
    {
    }
}