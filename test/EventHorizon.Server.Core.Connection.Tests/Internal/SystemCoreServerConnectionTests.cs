namespace EventHorizon.Server.Core.Connection.Tests.Internal
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using EventHorizon.Server.Core.Connection.Internal;

    using FluentAssertions;

    using Microsoft.AspNetCore.Connections;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.AspNetCore.SignalR.Protocol;
    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    /// <summary>
    /// Since the SystemCoreServerConnection is just a wrapper around the HubConnection for 
    ///  SignalR we are just checking that the logging is correctly done on Exceptions.
    /// </summary>
    public class SystemCoreServerConnectionTests
    {
        [Fact]
        public void ShouldLogErrorWhenGenericActionIsCalled()
        {
            // Given
            var actionName = "action-name";
            void actionToRun(TestResult result) { }

            var loggerMock = new Mock<ILogger>();
            var hubConnectionMock = new Mock<HubConnectionMock>();

            hubConnectionMock.Setup(
                // string methodName, Type[] parameterTypes, Func<object?[], object, Task> handler, object state
                mock => mock.On(
                    It.IsAny<string>(),
                    It.IsAny<Type[]>(),
                    It.IsAny<Func<object?[], object, Task>>(),
                    It.IsAny<object>()
                )
            ).Throws(
                new Exception("error")
            );

            // When
            var connection = new SystemCoreServerConnection(
                loggerMock.Object,
                hubConnectionMock.Object
            );
            Action action = () => connection.OnAction<TestResult>(
                actionName,
                actionToRun
            );

            // Then
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void ShouldLogErrorWhenActionIsCalled()
        {
            // Given
            var actionName = "action-name";
            void actionToRun() { }

            var loggerMock = new Mock<ILogger>();
            var hubConnectionMock = new Mock<HubConnectionMock>();

            hubConnectionMock.Setup(
                // string methodName, Type[] parameterTypes, Func<object?[], object, Task> handler, object state
                mock => mock.On(
                    It.IsAny<string>(),
                    It.IsAny<Type[]>(),
                    It.IsAny<Func<object?[], object, Task>>(),
                    It.IsAny<object>()
                )
            ).Throws(
                new Exception("error")
            );

            // When
            var connection = new SystemCoreServerConnection(
                loggerMock.Object,
                hubConnectionMock.Object
            );
            void action() => connection.OnAction(
                actionName,
                actionToRun
            );

            // Then
            Assert.Throws<Exception>(
                action
            );
        }

        [Fact]
        public async Task ShouldLogErrorWhenGenericSendActionIsCalled()
        {
            // Given
            var actionName = "action-name";
            var actionArgs = new[] { "arg1" };

            var loggerMock = new Mock<ILogger>();
            var hubConnectionMock = new Mock<HubConnectionMock>();

            // When
            var connection = new SystemCoreServerConnection(
                loggerMock.Object,
                hubConnectionMock.Object
            );
            async Task action() => await connection.SendAction<TestResult>(
                actionName,
                actionArgs
            );

            // Then
            await Assert.ThrowsAsync<NullReferenceException>(
                action
            );
        }

        [Fact]
        public async Task ShouldLogErrorWhenSendActionIsCalled()
        {
            // Given
            var actionName = "action-name";
            var actionArgs = new[] { "arg1" };

            var loggerMock = new Mock<ILogger>();
            var hubConnectionMock = new Mock<HubConnectionMock>();

            hubConnectionMock.Setup(
                // string methodName, Type returnType, object?[] args, CancellationToken cancellationToken = default(CancellationToken)
                mock => mock.InvokeCoreAsync(
                    It.IsAny<string>(),
                    It.IsAny<Type>(),
                    It.IsAny<object?[]>(),
                    It.IsAny<CancellationToken>()
                )
            ).Throws(
                new Exception("error")
            );

            // When
            var connection = new SystemCoreServerConnection(
                loggerMock.Object,
                hubConnectionMock.Object
            );
            async Task action() => await connection.SendAction(
                actionName,
                actionArgs
            );

            // Then
            await Assert.ThrowsAsync<Exception>(
                action
            );
        }

        public struct TestResult
        {

        }

        public class HubConnectionMock : HubConnection
        {
            public HubConnectionMock()
                : base(
                    new Mock<IConnectionFactory>().Object,
                    new Mock<IHubProtocol>().Object,
                    new Mock<EndPoint>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILoggerFactory>().Object
                )
            {

            }
        }
    }
}
