using EventHorizon.Game.Server.Zone.External.RandomNumber;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Services
{
    public interface IScriptServices
    {
        IMediator Mediator { get; }
        IRandomNumberGenerator Random { get; }
    }

    public class ScriptServices : IScriptServices
    {
        public IMediator Mediator { get; private set; }
        public IRandomNumberGenerator Random { get; private set; }
        public ScriptServices(
            IMediator mediator,
            IRandomNumberGenerator random
        )
        {
            Mediator = mediator;
            Random = random;
        }
    }
}