namespace EventHorizon.Zone.System.Server.Scripts.System
{
    using EventHorizon.Game.I18n;
    using EventHorizon.Zone.Core.Model.DateTimeService;
    using EventHorizon.Zone.Core.Model.Info;
    using EventHorizon.Zone.Core.Model.RandomNumber;
    using EventHorizon.Zone.System.Server.Scripts.Model;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class SystemServerScriptServices
        : ServerScriptServices
    {
        private readonly ILoggerFactory _loggerFactory;

        public ServerInfo ServerInfo { get; }
        public IMediator Mediator { get; }
        public IRandomNumberGenerator Random { get; }
        public IDateTimeService DateTime { get; }
        public I18nLookup I18n { get; }

        public SystemServerScriptServices(
            ServerInfo serverInfo,
            IMediator mediator,
            IRandomNumberGenerator random,
            IDateTimeService dateTime,
            I18nLookup i18n,
            ILoggerFactory loggerFactory
        )
        {
            ServerInfo = serverInfo;
            Mediator = mediator;
            Random = random;
            DateTime = dateTime;
            I18n = i18n;
            _loggerFactory = loggerFactory;
        }

        public ILogger Logger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }
    }
}