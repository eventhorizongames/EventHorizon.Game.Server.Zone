using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventHorizon.Plugin.Zone.System.Combat.Skill.Runner
{
    public class RunSkillWithTargetOfPositionHandler : INotificationHandler<RunSkillWithTargetOfPositionEvent>
    {
        public Task Handle(RunSkillWithTargetOfPositionEvent notification, CancellationToken cancellationToken)
        {
            throw new global::System.NotImplementedException();
        }
    }
}