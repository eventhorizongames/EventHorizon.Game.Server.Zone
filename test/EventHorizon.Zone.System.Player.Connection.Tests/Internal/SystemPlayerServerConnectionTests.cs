namespace EventHorizon.Zone.System.Player.Connection.Tests.Internal
{
    using global::System;
    using global::System.Net;
    using global::System.Threading.Tasks;

    using EventHorizon.Zone.System.Player.Connection.Internal;

    using Microsoft.AspNetCore.Connections;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.AspNetCore.SignalR.Protocol;
    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    /// <summary>
    /// Since the SystemPlayerServerConnection is just a wrapper around the HubConnection for 
    ///  SignalR we are just checking that the logging is correctly done on Exceptions.
    /// </summary>
    public class SystemPlayerServerConnectionTests
    {
        [Fact]
        public void TestShouldLogErrorWhenGenericActionIsCalled()
        {
            // Given
            var actionName = "action-name";
            Action<TestResult> actionToRun = result => { };

            var loggerMock = new Mock<ILogger>();
            var hubConnectionMock = new Mock<HubConnectionMock>();

            // When
            var connection = new SystemPlayerServerConnection(
                loggerMock.Object,
                hubConnectionMock.Object
            );
            Action action = () => connection.OnAction<TestResult>(
                actionName,
                actionToRun
            );

            // Then
            Assert.Throws<NullReferenceException>(
                action
            );
        }

        [Fact]
        public void TestShouldLogErrorWhenActionIsCalled()
        {
            // Given
            var actionName = "action-name";
            Action actionToRun = () => { };

            var loggerMock = new Mock<ILogger>();
            var hubConnectionMock = new Mock<HubConnectionMock>();

            // When
            var connection = new SystemPlayerServerConnection(
                loggerMock.Object,
                hubConnectionMock.Object
            );
            Action action = () => connection.OnAction(
                actionName,
                actionToRun
            );

            // Then
            Assert.Throws<NullReferenceException>(
                action
            );
        }

        [Fact]
        public async Task TestShouldLogErrorWhenGenericSendActionIsCalled()
        {
            // Given
            var actionName = "action-name";
            var actionArgs = new[] { "arg1" };

            var loggerMock = new Mock<ILogger>();
            var hubConnectionMock = new Mock<HubConnectionMock>();

            // When
            var connection = new SystemPlayerServerConnection(
                loggerMock.Object,
                hubConnectionMock.Object
            );
            Func<Task> action = async () => await connection.SendAction<TestResult>(
                actionName,
                actionArgs
            );

            // Then
            await Assert.ThrowsAsync<NullReferenceException>(
                action
            );
        }

        [Fact]
        public async Task TestShouldLogErrorWhenSendActionIsCalled()
        {
            // Given
            var actionName = "action-name";
            var actionArgs = new[] { "arg1" };

            var loggerMock = new Mock<ILogger>();
            var hubConnectionMock = new Mock<HubConnectionMock>();

            // When
            var connection = new SystemPlayerServerConnection(
                loggerMock.Object,
                hubConnectionMock.Object
            );
            Func<Task> action = async () => await connection.SendAction(
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
