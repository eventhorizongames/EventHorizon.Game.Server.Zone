﻿namespace EventHorizon.Server.Core.Tests.Register
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHorizon.Server.Core.Connection;
    using EventHorizon.Server.Core.Connection.Model;
    using EventHorizon.Server.Core.Events.Register;
    using EventHorizon.Server.Core.Register;
    using EventHorizon.Zone.Core.Model.ServerProperty;
    using EventHorizon.Zone.Core.Model.Settings;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class RegisterWithCoreServerHandlerTests
    {
        [Fact]
        public async Task ShouldSetServerIdRecievedFromRegisteredZoneCall()
        {
            // Given
            var expectedKey = ServerPropertyKeys.SERVER_ID;
            var expectedServerId = "server-id";
            var serverAddress = "server-address";
            var tag = "tag";

            var coreServerConnectionApiMock = new Mock<CoreServerConnectionApi>();

            var loggerMock = new Mock<ILogger<RegisterWithCoreServerHandler>>();
            var zoneSettings = new ZoneSettings
            {
                Tag = tag,
            };
            var serverPropertyMock = new Mock<IServerProperty>();
            var coreServerConnectionMock = new Mock<CoreServerConnection>();

            coreServerConnectionMock.Setup(
                mock => mock.Api
            ).Returns(
                coreServerConnectionApiMock.Object
            );

            coreServerConnectionApiMock.Setup(
                mock => mock.RegisterZone(
                    new ZoneRegistrationDetails(
                        serverAddress,
                        tag
                    )
                )
            ).ReturnsAsync(
                new RegisteredZoneDetails
                {
                    Id = expectedServerId
                }
            );

            serverPropertyMock.Setup(
                mock => mock.Get<string>(
                    ServerPropertyKeys.HOST
                )
            ).Returns(
                serverAddress
            );

            // When
            var handler = new RegisterWithCoreServerHandler(
                loggerMock.Object,
                zoneSettings,
                serverPropertyMock.Object,
                coreServerConnectionMock.Object
            );
            await handler.Handle(
                new RegisterWithCoreServer(),
                CancellationToken.None
            );

            // Then
            serverPropertyMock.Verify(
                mock => mock.Set(
                    expectedKey,
                    expectedServerId
                )
            );
        }

        [Fact]
        public async Task ShouldThrowExceptionWhenAnyExceptionIsThrown()
        {
            // Given
            var expected = "error-message";

            var coreServerConnectionApiMock = new Mock<CoreServerConnectionApi>();

            var loggerMock = new Mock<ILogger<RegisterWithCoreServerHandler>>();
            var zoneSettings = new ZoneSettings();
            var serverPropertyMock = new Mock<IServerProperty>();
            var coreServerConnectionMock = new Mock<CoreServerConnection>();

            coreServerConnectionMock.Setup(
                mock => mock.Api
            ).Returns(
                coreServerConnectionApiMock.Object
            );

            coreServerConnectionApiMock.Setup(
                mock => mock.RegisterZone(
                    It.IsAny<ZoneRegistrationDetails>()
                )
            ).Throws(
                new Exception(expected)
            );

            // When
            var handler = new RegisterWithCoreServerHandler(
                loggerMock.Object,
                zoneSettings,
                serverPropertyMock.Object,
                coreServerConnectionMock.Object
            );
            Func<Task> action = () => handler.Handle(
                new RegisterWithCoreServer(),
                CancellationToken.None
            );

            var actual = await Assert.ThrowsAsync<Exception>(
                action
            );

            // Then
            actual.Message
                .Should().Be(expected);
        }
    }
}