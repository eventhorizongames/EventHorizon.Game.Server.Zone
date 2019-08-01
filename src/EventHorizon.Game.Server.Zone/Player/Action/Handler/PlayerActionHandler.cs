using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json.Linq;
using EventHorizon.Game.Server.Zone.Agent.Companion.RunSkill;
using EventHorizon.Game.Server.Zone.External.Player;
using EventHorizon.Game.Server.Zone.Player.Actions;
using EventHorizon.Game.Server.Zone.Player.Actions.MovePlayer;
using EventHorizon.Plugin.Zone.System.Combat.Events.Skill.Runner;
using EventHorizon.Game.Server.Zone.Player.Actions.StopPlayer;
using EventHorizon.Zone.System.Player.Events.ClientAction;

namespace EventHorizon.Game.Server.Zone.Player.Action.Handler
{
    public class PlayerActionHandler : INotificationHandler<PlayerActionEvent>
    {
        readonly IMediator _mediator;
        readonly IPlayerRepository _playerRepository;

        public PlayerActionHandler(
            IMediator mediator,
            IPlayerRepository playerRepository
        )
        {
            _mediator = mediator;
            _playerRepository = playerRepository;
        }
        public async Task Handle(
            PlayerActionEvent notification,
            CancellationToken cancellationToken
        )
        {
            var player = await _playerRepository.FindById(notification.PlayerId);
            switch (notification.Action)
            {
                case PlayerActions.MOVE:
                    await _mediator.Send(new MovePlayerEvent()
                    {
                        Player = player,
                        MoveDirection = notification.Data
                    });
                    break;
                case PlayerActions.STOP:
                    await _mediator.Send(new StopPlayer(
                        player
                    ));
                    break;
                case PlayerActions.RUN_SKILL:
                    var parsedObject = GetDataAsRunSkillData(notification.Data);
                    await _mediator.Publish(
                        new RunSkillWithTargetOfEntityEvent
                        {
                            ConnectionId = player.ConnectionId,
                            CasterId = player.Id,
                            SkillId = parsedObject.skillId,
                            TargetId = parsedObject.targetId,
                            TargetPosition = parsedObject.targetPosition
                        }
                    );
                    break;
                case PlayerActions.RUN_SKILL_ON_COMPANION:
                    var parsedCompanionObject = GetDataAsRunSkillData(notification.Data);
                    await _mediator.Publish(
                        new RunPlayerCompanionSkillEvent(
                            player.ConnectionId,
                            player.Id,
                            parsedCompanionObject.casterId,
                            parsedCompanionObject.skillId,
                            parsedCompanionObject.targetId,
                            parsedCompanionObject.targetPosition
                        )
                    );
                    break;
                case PlayerActions.INTERACT:
                    var interactionEntityId = (long)notification.Data;
                    await _mediator.Publish(
                        new PlayerInteractionEvent(
                            player,
                            interactionEntityId
                        )
                    );
                    break;
            }
        }

        private RunSkillData GetDataAsRunSkillData(dynamic data)
        {
            var jObjectData = (JObject)data;
            var containsProp = jObjectData.ContainsKey("targetPosition");
            var runSkillData = jObjectData.ToObject<RunSkillData>();
            if (!jObjectData.ContainsKey("targetPosition"))
            {
                runSkillData.targetPosition = new Vector3(9_999_999, 9_999_999, 9_999_999);
            }
            return runSkillData;
        }

        private struct RunSkillData
        {
            public string skillId { get; set; }
            public long casterId { get; set; }
            public long targetId { get; set; }
            public Vector3 targetPosition { get; set; }
        }
    }
}