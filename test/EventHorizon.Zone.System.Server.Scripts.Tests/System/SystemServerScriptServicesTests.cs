﻿namespace EventHorizon.Zone.System.Server.Scripts.Tests.System;

using EventHorizon.Game.I18n;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.RandomNumber;
using EventHorizon.Zone.System.DataStorage.Model;
using EventHorizon.Zone.System.Server.Scripts.Model;
using EventHorizon.Zone.System.Server.Scripts.System;
using FluentAssertions;
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
        var mediatorMock = new Mock<ServerScriptMediator>();
        var randomMock = new Mock<IRandomNumberGenerator>();
        var dateTimeMock = new Mock<IDateTimeService>();
        var i18nMock = new Mock<I18nLookup>();
        var observerBrokerMock = new Mock<ServerScriptObserverBroker>();
        var dataStoreMock = new Mock<DataStore>();
        var dataParsersMock = new Mock<DataParsers>();
        var loggerFactoryMock = new Mock<ILoggerFactory>();

        // When
        var scriptServices = new SystemServerScriptServices(
            serverInfoMock.Object,
            mediatorMock.Object,
            randomMock.Object,
            dateTimeMock.Object,
            i18nMock.Object,
            observerBrokerMock.Object,
            dataStoreMock.Object,
            dataParsersMock.Object,
            loggerFactoryMock.Object
        );

        // Since we are dealing with the logging abstraction
        //  just make sure nothing changes with the abstraction.
        scriptServices.Logger<SystemServerScriptServicesTests>();

        // Then
        scriptServices.ServerInfo.Should().Be(serverInfoMock.Object);
        scriptServices.Mediator.Should().Be(mediatorMock.Object);
        scriptServices.Random.Should().Be(randomMock.Object);
        scriptServices.DateTime.Should().Be(dateTimeMock.Object);
        scriptServices.I18n.Should().Be(i18nMock.Object);
        scriptServices.DataStore.Should().Be(dataStoreMock.Object);
        scriptServices.DataParsers.Should().Be(dataParsersMock.Object);
        scriptServices.ObserverBroker.Should().Be(observerBrokerMock.Object);
    }
}
