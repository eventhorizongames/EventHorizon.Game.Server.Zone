using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System;
using EventHorizon.Game.Server.Zone.Agent.Move.Impl;
using Microsoft.Extensions.Logging;
using System.Threading;
using EventHorizon.Game.Server.Zone.Agent.Move;
using static EventHorizon.Game.Server.Zone.Agent.Move.Impl.MoveRegisteredAgentsTimer;

namespace EventHorizon.Game.Server.Zone.Tests.Agent.Move.Impl
{
    public class MoveRegisteredAgentsTimeTests : IDisposable
    {
        MoveRegisteredAgentsTimer moveRegisteredAgentsHandler;
        public void Dispose()
        {
            if (moveRegisteredAgentsHandler != null)
            {
                moveRegisteredAgentsHandler.Stop();
            }
        }

        [Fact]
        public void TestStart_ShouldPublishMoveRegisteredAgentsEventAfterSetAmountOfTime()
        {
            // Given
            var expectedMoveRegisteredAgentsEvent = new MoveRegisteredAgentsEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            var loggerMock = new Mock<ILogger<MoveRegisteredAgentsTimer>>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.Setup(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            // When
            moveRegisteredAgentsHandler = new MoveRegisteredAgentsTimer(
                loggerMock.Object,
                serviceScopeFactoryMock.Object
            );

            moveRegisteredAgentsHandler.Start();

            // Wait 200 ms, this will allow for callback of timer to be called
            // Callback should be ~100 ms
            Thread.Sleep(125);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish(expectedMoveRegisteredAgentsEvent, CancellationToken.None), Times.AtLeast(1));
        }

        [Fact]
        public void TestStop_ShouldStopMoveRegisteredAgentsEventAfterCalledToStop()
        {
            // Given
            var expectedMoveRegisteredAgentsEvent = new MoveRegisteredAgentsEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            var loggerMock = new Mock<ILogger<MoveRegisteredAgentsTimer>>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.Setup(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            // When
            moveRegisteredAgentsHandler = new MoveRegisteredAgentsTimer(
                loggerMock.Object,
                serviceScopeFactoryMock.Object
            );

            moveRegisteredAgentsHandler.Start();

            // Wait 200 ms, this will allow for callback of timer to be called
            // Callback should be ~100 ms
            Thread.Sleep(90);
            mediatorMock.Verify(mediator => mediator.Publish(expectedMoveRegisteredAgentsEvent, CancellationToken.None), Times.AtLeast(1));
            Thread.Sleep(90);
            mediatorMock.Verify(mediator => mediator.Publish(expectedMoveRegisteredAgentsEvent, CancellationToken.None), Times.AtMost(2));
            moveRegisteredAgentsHandler.Stop(); // Call Stop

            // Then
            Thread.Sleep(125);
            mediatorMock.Verify(mediator => mediator.Publish(expectedMoveRegisteredAgentsEvent, CancellationToken.None), Times.AtMost(2));
            Thread.Sleep(125);
            mediatorMock.Verify(mediator => mediator.Publish(expectedMoveRegisteredAgentsEvent, CancellationToken.None), Times.AtMost(2));
            Thread.Sleep(125);
            mediatorMock.Verify(mediator => mediator.Publish(expectedMoveRegisteredAgentsEvent, CancellationToken.None), Times.AtMost(2));
        }

        [Fact]
        public void TestStop_ShouldNotThrowExceptionOnStopWhenNotStarted()
        {
            // Given
            var loggerMock = new Mock<ILogger<MoveRegisteredAgentsTimer>>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            // When
            moveRegisteredAgentsHandler = new MoveRegisteredAgentsTimer(
                loggerMock.Object,
                serviceScopeFactoryMock.Object
            );

            moveRegisteredAgentsHandler.Stop();

            // Then
            Assert.True(true);
        }

        [Fact]
        public void TestOnMoveRegisteredAgents_ShouldCallMoveRegisteredAgentsEventWhenCalled()
        {
            // Given
            var inputTimerState = new TimerState();
            var expectedMoveRegisteredAgentsEvent = new MoveRegisteredAgentsEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            var loggerMock = new Mock<ILogger<MoveRegisteredAgentsTimer>>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.Setup(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            // When
            moveRegisteredAgentsHandler = new MoveRegisteredAgentsTimer(
                loggerMock.Object,
                serviceScopeFactoryMock.Object
            );

            moveRegisteredAgentsHandler.OnMoveRegisteredAgents(inputTimerState);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish(expectedMoveRegisteredAgentsEvent, CancellationToken.None), Times.AtLeast(1));
        }

        [Fact]
        public void TestOnMoveRegisteredAgents_ShouldLogWarningWhenTimerIsRuning()
        {
            // Given
            var inputTimerState = new TimerState
            {
                IsRunning = true
            };
            var expectedMoveRegisteredAgentsEvent = new MoveRegisteredAgentsEvent();

            var mediatorMock = new Mock<IMediator>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var serviceProviderMock = new Mock<IServiceProvider>();

            var loggerMock = new Mock<ILogger<MoveRegisteredAgentsTimer>>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

            serviceProviderMock.Setup(serviceProvider => serviceProvider.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeMock.Setup(serviceScope => serviceScope.ServiceProvider).Returns(serviceProviderMock.Object);
            serviceScopeFactoryMock.Setup(serviceScopeFactory => serviceScopeFactory.CreateScope()).Returns(serviceScopeMock.Object);

            // When
            moveRegisteredAgentsHandler = new MoveRegisteredAgentsTimer(
                loggerMock.Object,
                serviceScopeFactoryMock.Object
            );

            moveRegisteredAgentsHandler.OnMoveRegisteredAgents(inputTimerState);

            // Then
            mediatorMock.Verify(mediator => mediator.Publish(expectedMoveRegisteredAgentsEvent, CancellationToken.None), Times.Never());
        }

    }
}