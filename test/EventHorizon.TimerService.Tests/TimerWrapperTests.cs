using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using EventHorizon.Tests.TestUtils;
using System.Threading;
using MediatR;
using System;
using static EventHorizon.TimerService.TimerWrapper;
using Microsoft.Extensions.Logging.Internal;
using System.Threading.Tasks;

namespace EventHorizon.TimerService.Tests.TimerService
{
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
        public void TestOnExceptionDuringCallToOnRunEventShouldLogErrorMessage()
        {
            // Given
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
            loggerMock.Verify(
                mock => mock.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(
                        value => value.ToString()
                            .StartsWith(
                                $"Timer Exception. \nTimerState: \n | Id: {timerState.Id} \n | Guid: {timerState.Guid} \n | Tag: {tag}"
                            )
                    ),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                )
            );
        }
        [Fact]
        public void TestOnLongRunningCallShouldLogWarningMessage()
        {
            // Given
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
            loggerMock.Verify(
                mock => mock.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<FormattedLogValues>(
                        value => value.ToString()
                            .StartsWith(
                                $"Timer ran long. \nTimerState: \n | Id: {timerState.Id} \n | Guid: {timerState.Guid} \n | Tag: {tag}"
                            )
                    ),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()
                )
            );
        }
    }
}