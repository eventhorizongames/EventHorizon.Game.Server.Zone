using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System;
using EventHorizon.Tests.TestUtils;

namespace EventHorizon.TimerService.Tests.TimerService
{
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
            
            var serviceMocks = ServicesBuilderFactory.CreateServices();
            serviceMocks.ServiceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
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
                serviceMocks.ServiceScopeFactoryMock.Object,
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
}