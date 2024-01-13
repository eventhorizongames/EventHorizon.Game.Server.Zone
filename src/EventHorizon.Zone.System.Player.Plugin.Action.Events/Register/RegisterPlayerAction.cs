namespace EventHorizon.Zone.System.Player.Plugin.Action.Events.Register;

using EventHorizon.Zone.System.Player.Plugin.Action.Model;

using MediatR;

public struct RegisterPlayerAction
    : IRequest
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
