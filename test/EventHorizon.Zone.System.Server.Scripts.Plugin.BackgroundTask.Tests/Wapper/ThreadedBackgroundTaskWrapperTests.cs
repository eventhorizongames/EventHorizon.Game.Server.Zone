namespace EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Tests.Wapper
{
    using AutoFixture.Xunit2;

    using EventHorizon.Test.Common.Attributes;
    using EventHorizon.Test.Common.Utils;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Model;
    using EventHorizon.Zone.System.Server.Scripts.Plugin.BackgroundTask.Wrapper;

    using FluentAssertions;

    using global::System;
    using global::System.Threading;
    using global::System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class ThreadedBackgroundTaskWrapperTests
        : IDisposable
    {
        ThreadedBackgroundTaskWrapper _taskWrapper;

        public void Dispose()
        {
            _taskWrapper?.Stop();
        }

        [Theory, AutoServiceProviderData]
        [Trait("Category", "Integration")]
        public async Task ShouldPublishEventWhenSetAmountOfTimePasses(
            // Given
            ServiceScopeFactoryMock serviceScopeFactoryMock,
            [Frozen] Mock<ScriptedBackgroundTask> backgroundTaskMock,
            [Frozen] Mock<ServerScriptServices> scriptServicesMock,
            ThreadedBackgroundTaskWrapper taskWrapper
        )
        {
            var period = 10;
            _taskWrapper = taskWrapper;

            serviceScopeFactoryMock.WithMock(
                scriptServicesMock
            );

            backgroundTaskMock.Setup(
                mock => mock.TaskPeriod
            ).Returns(
                period
            );

            // When
            _taskWrapper.Start();

            await Task.Delay(15);

            // Then
            backgroundTaskMock.Verify(
                mock => mock.TaskTrigger(
                    scriptServicesMock.Object
                ),
                Times.AtLeastOnce()
            );
        }

        [Theory, AutoServiceProviderData]
        [Trait("Category", "Integration")]
        public async Task ShouldStopFutureEventCallsWhenStoppedAsync(
            // Given
            ServiceScopeFactoryMock serviceScopeFactoryMock,
            [Frozen] Mock<ScriptedBackgroundTask> backgroundTaskMock,
            [Frozen] Mock<ServerScriptServices> scriptServicesMock,
            ThreadedBackgroundTaskWrapper taskWrapper
        )
        {
            // Given
            var period = 120;
            _taskWrapper = taskWrapper;

            serviceScopeFactoryMock.WithMock(
                scriptServicesMock
            );

            backgroundTaskMock.Setup(
                mock => mock.TaskPeriod
            ).Returns(
                period
            );

            // When
            _taskWrapper.Start();

            // Wait 200 ms, this will allow for callback of timer to be called
            // Callback should be ~100 ms
            await Task.Delay(90);
            backgroundTaskMock.Verify(
                mock => mock.TaskTrigger(
                    scriptServicesMock.Object
                ),
                Times.AtLeast(1)
            );
            await Task.Delay(90);
            backgroundTaskMock.Verify(
                mock => mock.TaskTrigger(
                    scriptServicesMock.Object
                ),
                Times.AtMost(2)
            );
            _taskWrapper.Stop();

            // Then
            await Task.Delay(125);
            backgroundTaskMock.Verify(
                mock => mock.TaskTrigger(
                    scriptServicesMock.Object
                ),
                Times.AtMost(2)
            );
            await Task.Delay(125);
            backgroundTaskMock.Verify(
                mock => mock.TaskTrigger(
                    scriptServicesMock.Object
                ),
                Times.AtMost(2)
            );
            await Task.Delay(125);
            backgroundTaskMock.Verify(
                mock => mock.TaskTrigger(
                    scriptServicesMock.Object
                ),
                Times.AtMost(2)
            );
        }

        [Theory, AutoMoqData]
        public void ShouldNotThrowExceptionOnStopWhenNotStarted(
            // Given
            ThreadedBackgroundTaskWrapper taskWrapper
        )
        {
            _taskWrapper = taskWrapper;

            // When
            _taskWrapper.Stop();

            // Then
            Assert.True(true);
        }

        [Theory, AutoServiceProviderData]
        public void ShouldCallExpectedEventWhenStartedAndTimerHasFired(
            // Given
            ServiceScopeFactoryMock serviceScopeFactoryMock,
            [Frozen] Mock<ScriptedBackgroundTask> backgroundTaskMock,
            [Frozen] Mock<ServerScriptServices> scriptServicesMock,
            ThreadedBackgroundTaskWrapper taskWrapper
        )
        {
            var taskState = new BackgroundTaskWrapperState();
            _taskWrapper = taskWrapper;

            serviceScopeFactoryMock.WithMock(
                scriptServicesMock
            );

            // When
            _taskWrapper.OnRunTask(
                taskState
            );

            // Then
            backgroundTaskMock.Verify(
                mock => mock.TaskTrigger(
                    scriptServicesMock.Object
                ),
                Times.AtLeastOnce()
            );
        }

        [Theory, AutoServiceProviderData]
        public async Task ShouldNotGetPastTimerStateLockWhenTimerStateLockIsAlreadyLocked(
            // Given
            ServiceScopeFactoryMock serviceScopeFactoryMock,
            [Frozen] Mock<ServerScriptServices> scriptServicesMock,
            ThreadedBackgroundTaskWrapper taskWrapper
        )
        {
            var taskState = new BackgroundTaskWrapperState();
            _taskWrapper = taskWrapper;

            serviceScopeFactoryMock.WithMock(
                scriptServicesMock
            );

            // When
            // Grab the lock on the timer
            await taskState.LOCK.WaitAsync(0);
            _taskWrapper.OnRunTask(
                taskState
            );

            // Then
            serviceScopeFactoryMock.Verify(
                mock => mock.CreateScope(),
                Times.Never()
            );
        }

        [Theory, AutoServiceProviderData]
        public void ShouldNotTriggerTaskWhenStateIsRuning(
            // Given
            ServiceScopeFactoryMock serviceScopeFactoryMock,
            [Frozen] Mock<ScriptedBackgroundTask> backgroundTaskMock,
            [Frozen] Mock<ServerScriptServices> scriptServicesMock,
            ThreadedBackgroundTaskWrapper taskWrapper
        )
        {
            var taskState = new BackgroundTaskWrapperState
            {
                IsRunning = true
            };
            _taskWrapper = taskWrapper;

            serviceScopeFactoryMock.WithMock(
                scriptServicesMock
            );

            // When
            _taskWrapper.OnRunTask(
                taskState
            );

            // Then
            backgroundTaskMock.Verify(
                mock => mock.TaskTrigger(
                    It.IsAny<ServerScriptServices>()
                ),
                Times.Never()
            );
        }

        [Theory, AutoServiceProviderData]
        public void ShouldNotBubbleExceptionAndStillFinishWhenExceptionisThrownOnEventFiring(
            // Given
            ServiceScopeFactoryMock serviceScopeFactoryMock,
            [Frozen] Mock<ScriptedBackgroundTask> backgroundTaskMock,
            [Frozen] Mock<ServerScriptServices> scriptServicesMock,
            ThreadedBackgroundTaskWrapper taskWrapper
        )
        {
            var taskState = new BackgroundTaskWrapperState();
            _taskWrapper = taskWrapper;

            serviceScopeFactoryMock.WithMock(
                scriptServicesMock
            );

            backgroundTaskMock.Setup(
                mock => mock.TaskTrigger(
                    It.IsAny<ServerScriptServices>()
                )
            ).ThrowsAsync(
                new Exception(
                    "error message"
                )
            );

            // When
            _taskWrapper.OnRunTask(
                taskState
            );

            // Then
            taskState.ErrorsCaught
                .Should().Be(1);
        }

        [Theory, AutoServiceProviderData]
        [Trait("Category", "Integration")]
        public void OnLongRunningCallShouldLogWarningMessage(
            // Given
            [Frozen] Mock<ILogger> loggerMock,
            ServiceScopeFactoryMock serviceScopeFactoryMock,
            [Frozen] Mock<ScriptedBackgroundTask> backgroundTaskMock,
            [Frozen] Mock<ServerScriptServices> scriptServicesMock,
            ThreadedBackgroundTaskWrapper taskWrapper
        )
        {
            var taskState = new BackgroundTaskWrapperState();
            var period = 10;
            _taskWrapper = taskWrapper;

            serviceScopeFactoryMock.WithMock(
                scriptServicesMock
            );

            backgroundTaskMock.Setup(
                mock => mock.TaskPeriod
            ).Returns(
                period
            );

            backgroundTaskMock.Setup(
                mock => mock.TaskTrigger(
                    It.IsAny<ServerScriptServices>()
                )
            ).Callback(
                () => Thread.Sleep(100)
            ).Returns(
                Task.CompletedTask
            );

            // When
            _taskWrapper.OnRunTask(
                taskState
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

        [Theory, AutoMoqData]
        public void ShouldNotThrowExceptionWhenStateIsNull(
            // Given
            ThreadedBackgroundTaskWrapper taskWrapper
        )
        {
            _taskWrapper = taskWrapper;

            // When
            _taskWrapper.OnRunTask(
                null
            );

            // Then
            Assert.True(true);
        }

        [Theory, AutoMoqData]
        public void ShouldCompleteSuccessfullyWhenLifecycleMethodsAreCalled(
            // Given
            ThreadedBackgroundTaskWrapper taskWrapper
        )
        {
            _taskWrapper = taskWrapper;

            // When
            _taskWrapper.Start();
            _taskWrapper.Resume();
            _taskWrapper.Stop();
            _taskWrapper.Dispose();

            // Then
            Assert.True(true);
        }
    }
}
