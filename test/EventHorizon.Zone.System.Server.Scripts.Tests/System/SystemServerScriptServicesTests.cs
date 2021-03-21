namespace EventHorizon.Zone.System.Server.Scripts.Tests.System
{
    using EventHorizon.Game.I18n;
    using EventHorizon.Observer.State;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.RandomNumber;
    using EventHorizon.Zone.System.Server.Scripts.System;
    using FluentAssertions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;


    public class SystemServerScriptServicesTests
    {
        [Fact]
        public void ShouldProxyAllConstrucorArgumentsWhenServicesArePassedIntoConstructor()
        {
            // Given
            var serverInfoMock = new Mock<ServerInfo>();
            var mediatorMock = new Mock<IMediator>();
            var randomMock = new Mock<IRandomNumberGenerator>();
            var dateTimeMock = new Mock<IDateTimeService>();
            var i18nMock = new Mock<I18nLookup>();
            var observerStateMock = new Mock<ObserverState>();
            var loggerFactoryMock = new Mock<ILoggerFactory>();

            // When
            var scriptServices = new SystemServerScriptServices(
                serverInfoMock.Object,
                mediatorMock.Object,
                randomMock.Object,
                dateTimeMock.Object,
                i18nMock.Object,
                observerStateMock.Object,
                loggerFactoryMock.Object
            );

            // Since we are dealing with the logging abstraction
            //  just make sure nothing changes with the abstraction.
            scriptServices.Logger<SystemServerScriptServicesTests>();

            // Then
            scriptServices.ServerInfo
                .Should().Be(serverInfoMock.Object);
            scriptServices.Mediator
                .Should().Be(mediatorMock.Object);
            scriptServices.Random
                .Should().Be(randomMock.Object);
            scriptServices.DateTime
                .Should().Be(dateTimeMock.Object);
            scriptServices.I18n
                .Should().Be(i18nMock.Object);
            scriptServices.ObserverState
                .Should().Be(observerStateMock.Object);
        }
    }
}
