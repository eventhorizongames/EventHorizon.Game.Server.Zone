namespace EventHorizon.TimerService.Tests.TimerService;

using System;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using MediatR;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class TimerWrapperTests
    : IDisposable
{
    TimerWrapper _timerWrapper;

    public void Dispose()
    {
        _timerWrapper?.Stop();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public void ShouldPublishEventWhenSetAmountOfTimePasses()
    {
        // Given
        var period = 10;
        var expected = new TestNotificationEvent();

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
            expected
        );
        timerTaskMock.Setup(
            mock => mock.Period
        ).Returns(
            period
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
                expected,
                CancellationToken.None
            ),
            Times.AtLeastOnce()
        );
    }

    [Fact]
    [Trait("Category", "Integration")]
    public void ShouldStopFutureEventCallsWhenStopped()
    {
        // Given
        var period = 120;
        var expected = new TestNotificationEvent();

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
            expected
        );
        timerTaskMock.Setup(
            mock => mock.Period
        ).Returns(
            period
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
                expected,
                CancellationToken.None
            ),
            Times.AtLeast(1)
        );
        Thread.Sleep(90);
        mediatorMock.Verify(
            mock => mock.Publish<INotification>(
                expected,
                CancellationToken.None
            ),
            Times.AtMost(2)
        );
        _timerWrapper.Stop(); // Call Stop

        // Then
        Thread.Sleep(125);
        mediatorMock.Verify(
            mock => mock.Publish<INotification>(
                expected,
                CancellationToken.None
            ),
            Times.AtMost(2)
        );
        Thread.Sleep(125);
        mediatorMock.Verify(
            mock => mock.Publish<INotification>(
                expected,
                CancellationToken.None
            ),
            Times.AtMost(2)
        );
        Thread.Sleep(125);
        mediatorMock.Verify(
            mock => mock.Publish<INotification>(
                expected,
                CancellationToken.None
            ),
            Times.AtMost(2)
        );
    }

    [Fact]
    public void ShouldNotThrowExceptionOnStopWhenNotStarted()
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
    public void ShouldCallExpectedEventWhenStartedAndTimerHasFired()
    {
        // Given
        var timerState = new TimerState();
        var period = 100;
        var expected = new TestNotificationEvent();

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
            expected
        );
        timerTaskMock.Setup(
            mock => mock.Period
        ).Returns(
            period
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
        mediatorMock.Verify(
            mock => mock.Publish<INotification>(
                expected,
                CancellationToken.None
            ),
            Times.AtLeastOnce()
        );
    }

    [Fact]
    public void ShouldCallExpectedEventWhenStartedAndTimerHasFiredAndLogDetailsTrue()
    {
        // Given
        var timerState = new TimerState();
        var period = 100;
        var logDetails = true;
        var expected = new TestNotificationEvent();

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
            expected
        );
        timerTaskMock.Setup(
            mock => mock.Period
        ).Returns(
            period
        );
        timerTaskMock.Setup(
            mock => mock.LogDetails
        ).Returns(
            logDetails
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
        mediatorMock.Verify(
            mock => mock.Publish<INotification>(
                expected,
                CancellationToken.None
            ),
            Times.AtLeastOnce()
        );
    }


    [Fact]
    public void ShouldNotNotCallEventWhenOnValidationEventReturnsFalse()
    {
        // Given
        var timerState = new TimerState();
        var period = 100;
        var onValidationEventResponse = false;
        var expected = new TestNotificationEvent();

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
            expected
        );
        timerTaskMock.Setup(
            mock => mock.Period
        ).Returns(
            period
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
            timerState
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Publish<INotification>(
                expected,
                CancellationToken.None
            ),
            Times.Never()
        );
    }

    [Fact]
    public async Task ShouldNotGetPastTimerStateLockWhenTimerStateLockIsAlreadyLocked()
    {
        // Given
        var timerState = new TimerState();

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

        // Grab the lock on the timer
        await timerState.LOCK.WaitAsync(0);

        _timerWrapper.OnRunTask(
            timerState
        );

        // Then
        serviceScopeFactoryMock.Verify(
            mock => mock.CreateScope(),
            Times.Never()
        );
    }

    [Fact]
    public void ShouldCallMoveRegisteredAgentsEventWhenOnValidationEventReturnTrue()
    {
        // Given
        var timerState = new TimerState();
        var period = 100;
        var onValidationEventResponse = true;
        var expected = new TestNotificationEvent();

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
            expected
        );
        timerTaskMock.Setup(
            mock => mock.Period
        ).Returns(
            period
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
            timerState
        );

        // Then
        mediatorMock.Verify(
            mock => mock.Publish<INotification>(
                expected,
                CancellationToken.None
            ),
            Times.AtLeastOnce()
        );
    }

    [Fact]
    public void ShouldLogWarningWhenTimerIsRuning()
    {
        // Given
        var timerState = new TimerState
        {
            IsRunning = true
        };
        var period = 100;
        var expected = new TestNotificationEvent();

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
            expected
        );
        timerTaskMock.Setup(
            mock => mock.Period
        ).Returns(
            period
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
        var timerState = new TimerState();
        var tag = "tag";
        var period = 100;
        var expected = 1;

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
            period
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
        timerState.ErrorsCaught
            .Should().Be(expected);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public void OnLongRunningCallShouldLogWarningMessage()
    {
        // Given
        var timerState = new TimerState();
        var tag = "tag";
        var period = 10;
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
            period
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
        // LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter
        loggerMock.Verify(
            mock => mock.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            )
        );
    }
}
