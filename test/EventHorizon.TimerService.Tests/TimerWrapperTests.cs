namespace EventHorizon.TimerService.Tests.TimerService
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class TimerWrapperTests : IDisposable
    {
        TimerWrapper _timerWrapper;

        public void Dispose()
        {
            _timerWrapper?.Stop();
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

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );
            serviceScopeMock.Setup(
                mock => mock.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            serviceScopeFactoryMock.Setup(
                mock => mock.CreateScope()
            ).Returns(
                serviceScopeMock.Object
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

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );

            _timerWrapper.Start();

            Thread.Sleep(15);

            // Then
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedPublishedEvent,
                    CancellationToken.None
                ),
                Times.AtLeastOnce()
            );
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

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );
            serviceScopeMock.Setup(
                mock => mock.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            serviceScopeFactoryMock.Setup(
                mock => mock.CreateScope()
            ).Returns(
                serviceScopeMock.Object
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
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedPublishedEvent,
                    CancellationToken.None
                ),
                Times.AtLeast(1)
            );
            Thread.Sleep(90);
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedPublishedEvent,
                    CancellationToken.None
                ),
                Times.AtMost(2)
            );
            _timerWrapper.Stop(); // Call Stop

            // Then
            Thread.Sleep(125);
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedPublishedEvent,
                    CancellationToken.None
                ),
                Times.AtMost(2)
            );
            Thread.Sleep(125);
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedPublishedEvent,
                    CancellationToken.None
                ),
                Times.AtMost(2)
            );
            Thread.Sleep(125);
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedPublishedEvent,
                    CancellationToken.None
                ),
                Times.AtMost(2)
            );
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

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );
            serviceScopeMock.Setup(
                mock => mock.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            serviceScopeFactoryMock.Setup(
                mock => mock.CreateScope()
            ).Returns(
                serviceScopeMock.Object
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

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );

            _timerWrapper.OnRunTask(
                inputTimerState
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedPublishedEvent,
                    CancellationToken.None
                ),
                Times.AtLeastOnce()
            );
        }

        [Fact]
        public void TestShouldNotCallMoveRegisteredAgentsEventWhenOnValidationEventReturnFalse()
        {
            // Given
            var inputTimerState = new TimerState();
            var inputPeriod = 100;
            var onValidationEventResponse = false;
            var expectedNeverPublishedEvent = new TestNotificationEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            var loggerMock = new Mock<ILogger>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var timerTaskMock = new Mock<ITimerTask>();
            var onValidationEventMock = new Mock<IRequest<bool>>();

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );
            serviceScopeMock.Setup(
                mock => mock.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            serviceScopeFactoryMock.Setup(
                mock => mock.CreateScope()
            ).Returns(
                serviceScopeMock.Object
            );

            timerTaskMock.Setup(
                mock => mock.OnRunEvent
            ).Returns(
                expectedNeverPublishedEvent
            );
            timerTaskMock.Setup(
                mock => mock.Period
            ).Returns(
                inputPeriod
            );
            timerTaskMock.Setup(
                mock => mock.OnValidationEvent
            ).Returns(
                onValidationEventMock.Object
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    onValidationEventMock.Object,
                    CancellationToken.None
                )
            ).ReturnsAsync(
                onValidationEventResponse
            );

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );

            _timerWrapper.OnRunTask(
                inputTimerState
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedNeverPublishedEvent,
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public async Task TestShouldNotGetPastTimerStateLockWhenTimerStateLockIsAlreadyLocked()
        {
            // Given
            var inputTimerState = new TimerState();
            // Grab the lock on the timer
            await inputTimerState.LOCK.WaitAsync(0);

            var loggerMock = new Mock<ILogger>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var timerTaskMock = new Mock<ITimerTask>();
            var onValidationEventMock = new Mock<IRequest<bool>>();

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );

            _timerWrapper.OnRunTask(
                inputTimerState
            );

            // Then
            serviceScopeFactoryMock.Verify(
                mock => mock.CreateScope(),
                Times.Never()
            );
        }

        [Fact]
        public void TestShouldCallMoveRegisteredAgentsEventWhenOnValidationEventReturnTrue()
        {
            // Given
            var inputTimerState = new TimerState();
            var inputPeriod = 100;
            var onValidationEventResponse = true;
            var expectedNeverPublishedEvent = new TestNotificationEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            var loggerMock = new Mock<ILogger>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var timerTaskMock = new Mock<ITimerTask>();
            var onValidationEventMock = new Mock<IRequest<bool>>();

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );
            serviceScopeMock.Setup(
                mock => mock.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            serviceScopeFactoryMock.Setup(
                mock => mock.CreateScope()
            ).Returns(
                serviceScopeMock.Object
            );

            timerTaskMock.Setup(
                mock => mock.OnRunEvent
            ).Returns(
                expectedNeverPublishedEvent
            );
            timerTaskMock.Setup(
                mock => mock.Period
            ).Returns(
                inputPeriod
            );
            timerTaskMock.Setup(
                mock => mock.OnValidationEvent
            ).Returns(
                onValidationEventMock.Object
            );

            mediatorMock.Setup(
                mock => mock.Send(
                    onValidationEventMock.Object,
                    CancellationToken.None
                )
            ).ReturnsAsync(
                onValidationEventResponse
            );

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );

            _timerWrapper.OnRunTask(
                inputTimerState
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    expectedNeverPublishedEvent,
                    CancellationToken.None
                ),
                Times.AtLeastOnce()
            );
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

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );
            serviceScopeMock.Setup(
                mock => mock.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            serviceScopeFactoryMock.Setup(
                mock => mock.CreateScope()
            ).Returns(
                serviceScopeMock.Object
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

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );

            _timerWrapper.OnRunTask(
                inputTimerState
            );

            // Then
            mediatorMock.Verify(
                mock => mock.Publish<INotification>(
                    It.IsAny<INotification>(),
                    CancellationToken.None
                ),
                Times.Never()
            );
        }

        [Fact]
        public void ShouldNotBubbleExceptionAndStillFinishWhenExceptionisThrownOnEventFiring()
        {
            // Given
            var expected = false;
            var timerState = new TimerState();
            var tag = "tag";
            var inputPeriod = 100;

            var loggerMock = new Mock<ILogger>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var timerTaskMock = new Mock<ITimerTask>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var mediatorMock = new Mock<IMediator>();

            timerTaskMock.Setup(
                mock => mock.OnRunEvent
            ).Returns(
                new TestNotificationEvent()
            );
            timerTaskMock.Setup(
                mock => mock.Period
            ).Returns(
                inputPeriod
            );
            timerTaskMock.Setup(
                mock => mock.Tag
            ).Returns(
                tag
            );

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );
            serviceScopeMock.Setup(
                mock => mock.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            serviceScopeFactoryMock.Setup(
                mock => mock.CreateScope()
            ).Returns(
                serviceScopeMock.Object
            );

            mediatorMock.Setup(
                mock => mock.Publish(
                    It.IsAny<INotification>(),
                    CancellationToken.None
                )
            ).ThrowsAsync(
                new Exception(
                    "error message"
                )
            );

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );
            _timerWrapper.OnRunTask(
                timerState
            );

            // Then
            Assert.Equal(
                expected,
                timerState.IsRunning
            );
        }

        [Fact]
        public void TestOnLongRunningCallShouldLogWarningMessage()
        {
            // Given
            var expected = false;
            var timerState = new TimerState();
            var tag = "tag";
            var inputPeriod = 10;
            var onRunEvent = new TestNotificationEvent();

            var loggerMock = new Mock<ILogger>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var timerTaskMock = new Mock<ITimerTask>();

            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var mediatorMock = new Mock<IMediator>();

            timerTaskMock.Setup(
                mock => mock.OnRunEvent
            ).Returns(
                onRunEvent
            );
            timerTaskMock.Setup(
                mock => mock.Period
            ).Returns(
                inputPeriod
            );
            timerTaskMock.Setup(
                mock => mock.Tag
            ).Returns(
                tag
            );

            serviceProviderMock.Setup(
                mock => mock.GetService(
                    typeof(IMediator)
                )
            ).Returns(
                mediatorMock.Object
            );
            serviceScopeMock.Setup(
                mock => mock.ServiceProvider
            ).Returns(
                serviceProviderMock.Object
            );
            serviceScopeFactoryMock.Setup(
                mock => mock.CreateScope()
            ).Returns(
                serviceScopeMock.Object
            );

            mediatorMock.Setup(
                mock => mock.Publish(
                    It.IsAny<INotification>(),
                    CancellationToken.None
                )
            ).Callback(
                () => Thread.Sleep(100)
            ).Returns(
                Task.CompletedTask
            );

            // When
            _timerWrapper = new TimerWrapper(
                loggerMock.Object,
                serviceScopeFactoryMock.Object,
                timerTaskMock.Object
            );
            _timerWrapper.OnRunTask(
                timerState
            );

            // Then
            Assert.Equal(
                expected,
                timerState.IsRunning
            );
        }
    }
}