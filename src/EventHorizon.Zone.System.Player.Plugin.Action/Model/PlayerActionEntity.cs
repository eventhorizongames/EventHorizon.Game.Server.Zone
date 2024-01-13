namespace EventHorizon.Zone.System.Player.Plugin.Action.Model;

public struct PlayerActionEntity
{
    public long Id { get; }
    public string ActionName { get; }
    public PlayerActionEvent ActionEvent { get; }

    public PlayerActionEntity(
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
