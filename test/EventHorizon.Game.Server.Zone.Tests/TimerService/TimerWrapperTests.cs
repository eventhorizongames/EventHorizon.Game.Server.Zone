using Xunit;
using Moq;
using EventHorizon.TimerService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using EventHorizon.Game.Server.Zone.Tests.TestUtil.Events;
using System.Threading;
using MediatR;
using System;
using static EventHorizon.TimerService.TimerWrapper;

namespace EventHorizon.Game.Server.Zone.Tests.TimerService
{
    public class TimerWrapperTests : IDisposable
    {
        TimerWrapper _timerWrapper;

        public void Dispose()
        {
            if (_timerWrapper != null)
            {
                _timerWrapper.Stop();
            }
        }

        [Fact]
        public void TestStart_ShouldPublishEventAfterSetAmountOfTime()
        {
            // Given
            var inputPeriod = 10;
            var expectedPublishedEvent = new TestNotificationEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            var loggerMock = new Mock<ILogger>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var timerTaskMock = new Mock<ITimerTask>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.Setup(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            timerTaskMock.Setup(a => a.OnRunEvent).Returns(expectedPublishedEvent);
            timerTaskMock.Setup(a => a.Period).Returns(inputPeriod);

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );

            _timerWrapper.Start();

            Thread.Sleep(15);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish<INotification>(expectedPublishedEvent, CancellationToken.None), Times.AtLeast(1));
        }

        [Fact]
        public void TestStop_ShouldStopEventCallsAfterStopped()
        {
            // Given
            var inputPeriod = 120;
            var expectedPublishedEvent = new TestNotificationEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            var loggerMock = new Mock<ILogger>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var timerTaskMock = new Mock<ITimerTask>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.Setup(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            timerTaskMock.Setup(a => a.OnRunEvent).Returns(expectedPublishedEvent);
            timerTaskMock.Setup(a => a.Period).Returns(inputPeriod);

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );

            _timerWrapper.Start();

            // Wait 200 ms, this will allow for callback of timer to be called
            // Callback should be ~100 ms
            Thread.Sleep(90);
            mediatorMock.Verify(mediator => mediator.Publish<INotification>(expectedPublishedEvent, CancellationToken.None), Times.AtLeast(1));
            Thread.Sleep(90);
            mediatorMock.Verify(mediator => mediator.Publish<INotification>(expectedPublishedEvent, CancellationToken.None), Times.AtMost(2));
            _timerWrapper.Stop(); // Call Stop

            // Then
            Thread.Sleep(125);
            mediatorMock.Verify(mediator => mediator.Publish<INotification>(expectedPublishedEvent, CancellationToken.None), Times.AtMost(2));
            Thread.Sleep(125);
            mediatorMock.Verify(mediator => mediator.Publish<INotification>(expectedPublishedEvent, CancellationToken.None), Times.AtMost(2));
            Thread.Sleep(125);
            mediatorMock.Verify(mediator => mediator.Publish<INotification>(expectedPublishedEvent, CancellationToken.None), Times.AtMost(2));
        }

        [Fact]
        public void TestStop_ShouldNotThrowExceptionOnStopWhenNotStarted()
        {
            // Given
            var loggerMock = new Mock<ILogger>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var timerTaskMock = new Mock<ITimerTask>();

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );

            _timerWrapper.Stop();

            // Then
            Assert.True(true);
        }

        [Fact]
        public void TestOnMoveRegisteredAgents_ShouldCallMoveRegisteredAgentsEventWhenCalled()
        {
            // Given
            var inputTimerState = new TimerState();
            var inputPeriod = 100;
            var expectedPublishedEvent = new TestNotificationEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            var loggerMock = new Mock<ILogger>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var timerTaskMock = new Mock<ITimerTask>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.Setup(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            timerTaskMock.Setup(a => a.OnRunEvent).Returns(expectedPublishedEvent);
            timerTaskMock.Setup(a => a.Period).Returns(inputPeriod);

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );

            _timerWrapper.OnRunTask(inputTimerState);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish<INotification>(expectedPublishedEvent, CancellationToken.None), Times.AtLeast(1));
        }

        [Fact]
        public void TestOnMoveRegisteredAgents_ShouldLogWarningWhenTimerIsRuning()
        {
            // Given
            var inputTimerState = new TimerState
            {
                IsRunning = true
            };
            var inputPeriod = 100;
            var expectedPublishedEvent = new TestNotificationEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            var loggerMock = new Mock<ILogger>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var timerTaskMock = new Mock<ITimerTask>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.Setup(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            timerTaskMock.Setup(a => a.OnRunEvent).Returns(expectedPublishedEvent);
            timerTaskMock.Setup(a => a.Period).Returns(inputPeriod);

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );

            _timerWrapper.OnRunTask(inputTimerState);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish<INotification>(It.IsAny<INotification>(), CancellationToken.None), Times.Never());
        }
    }
}