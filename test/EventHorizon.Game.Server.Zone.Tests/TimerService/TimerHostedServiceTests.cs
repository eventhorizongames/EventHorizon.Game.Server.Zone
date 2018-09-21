using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using EventHorizon.TimerService;
using System.Threading;
using System.Threading.Tasks;
using EventHorizon.Game.Server.Zone.Tests.TestUtil.Events;
using MediatR;
using System;

namespace EventHorizon.Game.Server.Zone.Tests.TimerService
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
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.Setup(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            timerTaskMock.Setup(a => a.OnRunEvent).Returns(expectedPublishedEvent);
            timerTaskMock.Setup(a => a.Period).Returns(inputPeriod);
            timerTasksMock.Add(timerTaskMock.Object);

            // When
            var timerHostedService = new TimerHostedService(
                loggerFactoryMock.Object,
                serviceScopeFactoryMock.Object,
                timerTasksMock
            );

            await timerHostedService.StartAsync(inputCancellationTokenSource.Token);

            Thread.Sleep(125);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish<INotification>(It.IsAny<INotification>(), CancellationToken.None), Times.AtLeastOnce());
            
            await timerHostedService.StopAsync(inputCancellationTokenSource.Token);
        }
    }
}