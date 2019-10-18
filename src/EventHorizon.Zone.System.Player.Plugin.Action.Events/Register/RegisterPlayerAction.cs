using EventHorizon.Zone.System.Player.Plugin.Action.Model;
using MediatR;

namespace EventHorizon.Zone.System.Player.Plugin.Action.Events.Register
{
    public struct RegisterPlayerAction : IRequest
    {
        public long Id { get; }
        public string ActionName { get; }
        public PlayerActionEvent ActionEvent { get; }

        public RegisterPlayerAction(
            long id,
            string actionName,
            PlayerActionEvent actionEvent
        )
        {
            Id = id;
            ActionName = actionName;
            ActionEvent = actionEvent;
        }
    }
}