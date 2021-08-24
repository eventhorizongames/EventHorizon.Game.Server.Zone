namespace EventHorizon.Server.Core.Connection.Tests.Internal
{
    using System;
    using System.Net;
    using System.Threading.Tasks;

    using EventHorizon.Server.Core.Connection.Internal;

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

            // When
            var connection = new SystemCoreServerConnection(
                loggerMock.Object,
                hubConnectionMock.Object
            );
            void action() => connection.OnAction<TestResult>(
                actionName,
                actionToRun
            );

            // Then
            Assert.Throws<NullReferenceException>(
                action
            );
        }

        [Fact]
        public void ShouldLogErrorWhenActionIsCalled()
        {
            // Given
            var actionName = "action-name";
            void actionToRun() { }

            var loggerMock = new Mock<ILogger>();
            var hubConnectionMock = new Mock<HubConnectionMock>();

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
            Assert.Throws<NullReferenceException>(
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
            await Assert.ThrowsAsync<NullReferenceException>(
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
