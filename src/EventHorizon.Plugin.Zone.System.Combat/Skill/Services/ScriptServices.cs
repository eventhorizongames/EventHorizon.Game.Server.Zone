using EventHorizon.Game.Server.Zone.External.DateTimeService;
using EventHorizon.Game.Server.Zone.External.RandomNumber;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Services
{
    public interface IScriptServices
    {
        IMediator Mediator { get; }
        IDateTimeService DateTime { get; }
        IRandomNumberGenerator Random { get; }
    }

    public class ScriptServices : IScriptServices
    {
        public IMediator Mediator { get; private set; }
        public IRandomNumberGenerator Random { get; private set; }
        public IDateTimeService DateTime { get; private set; }

        public ScriptServices(
            IMediator mediator,
            IDateTimeService dateTime,
            IRandomNumberGenerator random
        )
        {
            Mediator = mediator;
            DateTime = dateTime;
            Random = random;
        }
    }
}