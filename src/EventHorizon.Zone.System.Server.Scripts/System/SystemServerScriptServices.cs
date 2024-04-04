namespace EventHorizon.Zone.System.Server.Scripts.System;

using EventHorizon.Game.I18n;
using EventHorizon.Zone.Core.Model.DateTimeService;
using EventHorizon.Zone.Core.Model.Info;
using EventHorizon.Zone.Core.Model.RandomNumber;
using EventHorizon.Zone.System.DataStorage.Model;
using EventHorizon.Zone.System.Server.Scripts.Model;
using Microsoft.Extensions.Logging;

public class SystemServerScriptServices(
    ServerInfo serverInfo,
    ServerScriptMediator mediator,
    IRandomNumberGenerator random,
    IDateTimeService dateTime,
    I18nLookup i18n,
    ServerScriptObserverBroker observerBroker,
    DataStore dataStore,
    DataParsers dataParsers,
    ILoggerFactory loggerFactory
) : ServerScriptServices
{
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public ServerInfo ServerInfo { get; } = serverInfo;
    public ServerScriptMediator Mediator { get; } = mediator;
    public IRandomNumberGenerator Random { get; } = random;
    public IDateTimeService DateTime { get; } = dateTime;
    public I18nLookup I18n { get; } = i18n;
    public ServerScriptObserverBroker ObserverBroker { get; } = observerBroker;
    public DataStore DataStore { get; } = dataStore;
    public DataParsers DataParsers { get; } = dataParsers;

    public ILogger Logger<T>()
    {
        return _loggerFactory.CreateLogger<T>();
    }
}
